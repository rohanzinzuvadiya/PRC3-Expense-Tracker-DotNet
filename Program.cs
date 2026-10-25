using System;
using System.Collections.Generic;

namespace ExpenseTrackerModule
{
    // Custom exception for our own business rule
    class InvalidExpenseException : Exception
    {
        public InvalidExpenseException(string message) : base(message) { }
    }

    class Expense
    {
        public string Title;
        public double Amount;
        public string Category;
    }

    class Program
    {
        static List<Expense> expenses = new List<Expense>();
        const double MaxAmount = 100000;

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n===== EXPENSE TRACKER =====");
                Console.WriteLine("1. Add Expense");
                Console.WriteLine("2. View Expenses");
                Console.WriteLine("3. Show Total");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option (1-4): ");

                try
                {
                    int choice = int.Parse(Console.ReadLine());   // FormatException possible

                    switch (choice)
                    {
                        case 1: AddExpense(); break;
                        case 2: ViewExpenses(); break;
                        case 3: ShowTotal(); break;
                        case 4: running = false; break;
                        default:
                            throw new InvalidExpenseException("Menu option must be between 1 and 4.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: please enter numbers only.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Error: the number you entered is too large.");
                }
                catch (InvalidExpenseException ex)
                {
                    Console.WriteLine("Rule violated: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected error: " + ex.Message);
                }
                finally
                {
                    // always runs, whether an exception happened or not
                    if (running) Console.WriteLine("(Expenses saved so far: " + expenses.Count + ")");
                }
            }

            Console.WriteLine("Goodbye!");
        }

        static void AddExpense()
        {
            Console.Write("Title    : ");
            string title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidExpenseException("Title cannot be empty.");

            Console.Write("Category : ");
            string category = Console.ReadLine();

            Console.Write("Amount   : ");
            double amount = double.Parse(Console.ReadLine());      // FormatException possible

            if (amount <= 0)
                throw new InvalidExpenseException("Amount must be greater than zero.");
            if (amount > MaxAmount)
                throw new InvalidExpenseException("Amount cannot exceed Rs. " + MaxAmount);

            expenses.Add(new Expense { Title = title, Category = category, Amount = amount });
            Console.WriteLine("Expense added successfully.");
        }

        static void ViewExpenses()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses recorded yet.");
                return;
            }

            Console.WriteLine("\nNo  Title                Category        Amount");
            Console.WriteLine("-----------------------------------------------");
            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine("{0,-3} {1,-20} {2,-15} {3,8:F2}",
                    i + 1, expenses[i].Title, expenses[i].Category, expenses[i].Amount);
            }
        }

        static void ShowTotal()
        {
            double total = 0;
            foreach (Expense e in expenses) total += e.Amount;
            Console.WriteLine("Total spent: Rs. " + total.ToString("F2"));

            if (expenses.Count > 0)
            {
                // Divide-by-zero is avoided because the list is not empty
                Console.WriteLine("Average    : Rs. " + (total / expenses.Count).ToString("F2"));
            }
        }
    }
}
