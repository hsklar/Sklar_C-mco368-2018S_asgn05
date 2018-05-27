using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLApp
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal min = 0;
            decimal max = 0;
            char choice = '0';
            DataClasses1DataContext db = new DataClasses1DataContext();

            do
            {
                Console.WriteLine("Enter 1 to view a range of items, 2 to enter a new item, any other key to exit");
                choice = Convert.ToChar(Console.ReadLine());

                if (choice == '1')
                {
                    try
                    {
                        Console.WriteLine("Please enter the minimum price you'd like to view:");
                        min = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Please enter the maximum price you'd like to view:");
                        max = Convert.ToDecimal(Console.ReadLine());

                        db.PRODUCTs.Where(p => p.P_PRICE >= min && p.P_PRICE <= max).ToList().ForEach(
                          product => Console.WriteLine($"\nProduct: {product.P_CODE} \nDescription: {product.P_DESCRIPT}" +
                               $"\nPrice: {product.P_PRICE} \nQOH: {product.P_QOH} \nVendor Code: {product.V_CODE ?? null}" +
                               $" \nVendor Name: {product.VENDOR?.V_NAME ?? ""}  \nVendor Contact: {product.VENDOR?.V_CONTACT ?? ""}"));

                    }

                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid value");
                    }
                   
                }

                else if (choice == '2')
                {
                    try
                    {
                        Console.WriteLine("Enter the Product Code:");
                        string pCode = Console.ReadLine();
                        Console.WriteLine("Enter the Product Description:");
                        string desc = Console.ReadLine();
                        Console.WriteLine("Enter the Product Indate:");
                        DateTime indate = Convert.ToDateTime(Console.ReadLine());
                        Console.WriteLine("Enter the Product QOH:");
                        int qoh = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter the Product Min:");
                        int pMin = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter the Product Price:");
                        decimal price = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter the Product Discount:");
                        decimal discount = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Does the product's vendor already exist? Enter Y or N");
                        string vendorExist = Console.ReadLine().ToUpper();
                        VENDOR vndr;

                        if (vendorExist[0] == 'N')
                        {
                            vndr = addVendor();
                            Console.WriteLine("Vendor has  been added");
                        }

                        else
                        {
                            Console.WriteLine("Enter the Vendor Code");
                            int code = Convert.ToInt32(Console.ReadLine());
                            if (db.VENDORs.Any(v => v.V_CODE == code))
                            {
                                vndr = db.VENDORs.Where(vend => vend.V_CODE == code).First();
                            }
                            else
                            {
                                Console.WriteLine("That vendor does not exist. Please enter the " +
                                    "information to add it to the database");
                                vndr = addVendor();
                                Console.WriteLine("Vendor has  been added");
                            }
                        }

                        PRODUCT p = new PRODUCT()
                        {
                            P_CODE = pCode,
                            P_DESCRIPT = desc,
                            P_INDATE = indate,
                            P_QOH = qoh,
                            P_MIN = pMin,
                            P_PRICE = price,
                            P_DISCOUNT = discount,
                            V_CODE = vndr.V_CODE
                        };
                        db.PRODUCTs.InsertOnSubmit(p);
                        db.SubmitChanges();
                        Console.WriteLine("Product has been added");
                    }

                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid value\n");
                    }
                }
                
                else
                {
                    Environment.Exit(0);
                }
            } while (true);


        }

        public static VENDOR addVendor()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            Console.WriteLine("\nEnter the vendor code:");
            int code = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the vendor name:");
            String name = Console.ReadLine();
            Console.WriteLine("Enter the vendor contact:");
            String contact = Console.ReadLine();
            Console.WriteLine("Enter the vendor area code:");
            String area = Console.ReadLine();
            Console.WriteLine("Enter the vendor phone number:");
            String phone = Console.ReadLine();
            Console.WriteLine("Enter the vendor state:");
            String state = Console.ReadLine();
            Console.WriteLine("Enter y or n for the vendor order:");
            string order = Console.ReadLine();
            Console.WriteLine();
            VENDOR v = new VENDOR()
            {
                V_CODE = code,
                V_NAME = name,
                V_CONTACT = contact,
                V_AREACODE = area,
                V_PHONE = phone,
                V_STATE = state,
                V_ORDER = order[0]
            };

            db.VENDORs.InsertOnSubmit(v);
            db.SubmitChanges();
            return v;
        }
    }
}
