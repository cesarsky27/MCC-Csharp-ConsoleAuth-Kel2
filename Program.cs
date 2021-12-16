using System;
using System.Collections.Generic;
using System.Linq;


namespace AuthApp
{
    class Program
    {
        public static List<User> users = new List<User>();
        public static Auth auth = new Auth();
        static void Main(string[] args)
        {

            bool run = true;
            while (run)
            {
                Console.Clear();
                Menu();
                int choiceMenu;
                do
                {
                    try
                    {
                        choiceMenu = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Input tidak valid");
                        continue;
                    }
                    break;
                } while (true);

                switch (choiceMenu)
                {
                    case 0:
                        auth.CheckLogin();
                        break;
                    case 1:
                        users.Add(CreateUser());
                        break;
                    case 2:
                        EditUser();
                        break;
                    case 3:
                        DeleteUser();
                        break;
                    case 4:
                        ShowUser();
                        break;
                    case 5:
                        SearchUser();
                        break;
                    case 6:
                        if (!auth.IsLogin())
                        {
                          LoginMenu();
                          break;
                        }
                        else
                        {
                          auth.Logout();
                          break;
                        }   
                    case 99:
                        run = false;
                        Console.WriteLine("APLIKASI DITUTUP..");
                        Console.WriteLine("===================================");
                        Console.WriteLine("Terima Kasih telah menggunakan aplikasi kami");
                        Console.WriteLine("===================================");
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Menu()
        {
            Console.WriteLine("===================================");
            Console.WriteLine("========= Selamat datang ==========");
            if (auth.IsLogin())
            {
                Console.WriteLine("Halo, " + auth.FirstName);
            }
            Console.WriteLine("===================================");
            Console.WriteLine("Silahkan pilih menu:     ");
            Console.WriteLine("===================================");
            if (auth.IsLogin())
            {
                Console.WriteLine("0. Account");
            }
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Edit User");
            Console.WriteLine("3. Delete User");
            Console.WriteLine("4. Show User");
            Console.WriteLine("5. Search User");
            if(!auth.IsLogin())
            { 
            Console.WriteLine("6. Login");
            }
            else
            { 
            Console.WriteLine("6. Logout");
            }
            Console.WriteLine("99. Exit");
            Console.Write("=> ");
        }

        private static User CreateUser()
        {
            User user = new User();
            Console.Clear();
            do
            {
                try
                {
                    Console.WriteLine("1. Create User");
                    Console.WriteLine("===============");
                    Console.Write("Nama Depan: ");
                    user.FirstName = Console.ReadLine();
                    Console.Write("Nama Belakang: ");
                    user.LastName = Console.ReadLine();
                    Console.Write("Password: ");
                    string hasPassword = BCrypt.Net.BCrypt.HashPassword(Console.ReadLine());
                    user.Password = hasPassword;
                    user.SetUsername(users);

                    if (user.Validate(user.FirstName, user.LastName, user.Password))
                    {
                        Console.WriteLine("\nUser Created!");
                    }
                    else {
                        Console.WriteLine("\nInput tidak valid");
                        Console.WriteLine("\nTekan apa saja untuk kembali");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInput tidak valid");
                    Console.WriteLine("Tekan apa saja untuk melanjutkan");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                break;
            } while (true);

            Console.WriteLine("\nTekan apa saja untuk melanjutkan");
            Console.ReadKey();
            return user;
        }

        public static void ShowUser()
        {
            Console.Clear();
            Console.WriteLine("4. Show User");
            Console.WriteLine("===============");
            foreach (User user in users)
            {
                user.Details();
            }
            Console.WriteLine("\nTekan apa saja untuk kembali..");
            Console.ReadKey();
        }

        public static void SearchUser()
        {
            Console.Clear();
            Console.WriteLine ("5. Search User");
            Console.WriteLine("====================");
            Console.Write("Search : ");
            string searchQuery = Console.ReadLine().ToLower();
            List<User> SearchResult = new List<User>();
            //disini pake ignore case
            foreach (User user in users)
            {
                if (user.FirstName.ToLower().Contains(searchQuery) ||
                    user.LastName.ToLower().Contains(searchQuery) ||
                    user.UserName.ToLower().Contains(searchQuery))
                {
                    SearchResult.Add(user);
                }
            }

            if (SearchResult.Count > 0)
            {
                foreach (User user in SearchResult)
                {
                    user.Details();
                }
            }
            else
            {
                Console.WriteLine("User tidak ditemukan");
            }
            Console.WriteLine("\nTekan apa saja untuk kembali..");
            Console.ReadKey();
        }


        public static void LoginMenu()
        {
            if (auth.UserName == null)
            {
            Console.Clear();
            Console.WriteLine("6. Login");
            Console.WriteLine("===========");
            Console.Write("Username:");
            string username = Console.ReadLine();
            Console.Write("Password:");
            string password = Console.ReadLine();
            Console.WriteLine(auth.Login(username, password, users));
            Console.ReadKey();
            }
        }

        public static void EditUser()
        {
            User editUser = null;
            string enteredUsername;
            Console.Clear();
            Console.WriteLine("2. Edit User");
            Console.WriteLine("===============");
            Console.Write("Cari username :");
            do
            {
                try
                {
                    enteredUsername = Console.ReadLine();
                }
                catch (Exception)
                {
                    Console.WriteLine("Input tidak valid");
                    Console.Write("Username :");
                    continue;
                }
                break;
            } while (true);

            foreach (User user in users)
            {
                if (user.UserName == enteredUsername)
                {
                    editUser = user;
                } 
            }
            

            if (editUser != null)
            {
                do
                {
                    try
                    {
                        Console.WriteLine("User ditemukan!");
                        editUser.Details();

                        Console.WriteLine("=====EDIT=====");
                        Console.Write("Nama Depan: ");
                        string firstName = Console.ReadLine();
                        Console.Write("Nama Belakang: ");
                        string lastName = Console.ReadLine();
                        Console.Write("Password: ");
                        string hasPassword = BCrypt.Net.BCrypt.HashPassword(Console.ReadLine());
                        string password = hasPassword;

                        if (editUser.Validate(firstName, lastName, password))
                        {
                            editUser.FirstName = firstName;
                            editUser.LastName = lastName;
                            editUser.Password = password;
                            editUser.SetUsername(users);
                            Console.WriteLine("\nUser Created!");
                            if (enteredUsername == auth.UserName)
                            {
                                auth.FirstName = firstName;
                                auth.LastName = lastName;
                                auth.Password = password;
                                auth.UserName = editUser.UserName;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nInput tidak valid");
                            Console.WriteLine("Tekan apa saja untuk melanjutkan");
                            Console.ReadKey();
                            Console.Clear();
                            continue;
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nInput tidak valid");
                        Console.WriteLine("\nTekan apa saja untuk melanjutkan");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                    break;
                } while (true);

                Console.WriteLine("User Updated!");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("User tidak ditemukan");
            }

        }

        static void DeleteUser()
        {
            string deleteUser;
            string msg = "";
            Console.Clear();
            Console.WriteLine("");
            Console.Write("Masukkan Username yang ingin anda delete: ");
            deleteUser = Console.ReadLine();
            
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].UserName == deleteUser)
                {
                    if (auth.UserName != null)
                    {
                        if (auth.UserName == deleteUser)
                        {
                            Console.WriteLine("Username tidak bisa dihapus");
                            Console.ReadKey();
                            break;
                        }
                    }
                    Console.WriteLine("Username yang akan didelete: " + deleteUser);
                    if (auth != null)
                    {
                        if (auth.UserName == deleteUser)
                        {
                            auth = new Auth();
                        }
                    }
                    users.RemoveAt(i);
                    msg = "User berhasil didelete";
                }
                else
                {
                    msg = "Username yang ingin didelete tidak ada!";
                }
            }
            Console.WriteLine(msg);
            Console.WriteLine("\nTekan apa saja untuk kembali");
            Console.ReadLine();
            Console.Clear();
        }

    }
}
