using BLOG_CORE.Entity;
using BLOG_CORE.Helpers;
using BLOG_CORE.Repository;
using BLOG_CORE.ViewModels.Account;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BLOG_CORE.Model.Account
{
    public class Account
    {
        readonly AccountRepository accountRepository = new AccountRepository();
        
        //регистрация пользователя
        public string AccountCreate(User user, string password)
        {
            // валидация
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(user.LastName) || string.IsNullOrWhiteSpace(user.FirstName))
                throw new AppException("Не все поля заполнены");

            //проверка на существующий логин
            if (accountRepository.FindAll().Any(x => x.Username == user.Username))
                throw new AppException("Пользователь \"" + user.Username + "\" уже зарегистрирован");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            if (accountRepository.RegisterUser(user) != 1)
                throw new AppException("Ошибка создания нового пользователя");

            return "Пользователь \"" + user.Username + "\" успешно зарегистрирован";
        }

        //атутентификация пользователя и получение ролей
        public AccountVM Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new AppException("Не все поля заполнены");

            //получаем сущность юзера из бд
            var user = accountRepository.FindAll().SingleOrDefault(x => x.Username == username);
            //проверка на существующий логин
            if (user == null)
                throw new AppException("Пользователя не существует");

            AccountVM userVM = new AccountVM()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Roles = AccountRole(user),
            };

            //проверка на правильный пароль
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new AppException("Пароль не корректен");

            // authentication successful
            return userVM;
        }

        //получение ролей для пользователя
        public List<AccountRoleVM> AccountRole(User user)
        {
            var roleList = new List<AccountRoleVM>();
            //сущность пользователь_роль из бд
            var userRoleList = accountRepository.UserRoleGetByID(user);

            foreach (var userRole in userRoleList)
            {
                //сущность роль из бд
                var role = accountRepository.RoleGetByID(userRole);
                AccountRoleVM roleVM = new AccountRoleVM()
                {
                    Title = role.Title,
                };
                roleList.Add(roleVM);
            }
            return roleList;
        }

        //создание токена
        public string CreateToken(AccountVM user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SECRETCODE31122019SECRETCODE31122019SECRETCODE31122019");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("uniq_id", user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        //создание хэш'а пароля и соли
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //верификация введённого пароля
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
