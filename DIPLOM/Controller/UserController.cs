using DIPLOM.Infrastructure;
using DIPLOM.Model;
using System;
using System.Linq;
using System.Windows;

namespace DIPLOM.Controller
{
    class UserController
    {
        static DB_Context DB;

        public UserController(DB_Context db)
        {
            DB = db;
        }

        public void Add(string login, string pass, Role role)
        {
            if (Checkers.Firmness(login) && Checkers.Firmness(pass))
            {
                try
                {
                    User newRow = new User(login, Encrypting.ComputeHash(pass), role);
                    var equalRecords = DB.Users.Where(l => l.Login.Equals(login));
                    if (equalRecords.Any())
                    {
                        MessageBox.Show("Такой пользователь уже существует в базе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        DB.Users.Add(newRow);
                        DB.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка добавления\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void Edit(string login, string passHash, Role role, int rowId)
        {
            if (Checkers.Firmness(login) && Checkers.Firmness(passHash))
            {
                {
                    try
                    {
                        var EditedValue = DB.Users.Where(c => c.Id == rowId)
                            .FirstOrDefault();
                        EditedValue.Login = login;
                        EditedValue.PassHash = passHash;
                        EditedValue.Role = role;
                        DB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка редактирования\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        public void DeleteByIndex(int index)
        {
            try
            {
                User DelitedValue = DB.Users.Where(l => l.Id == index).FirstOrDefault();
                DB.Users.Remove(DelitedValue);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка удаления\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteLast()
        {
            try
            {
                User DelitedValue = DB.Users.Last();
                DB.Users.Remove(DelitedValue);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка удаления\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
