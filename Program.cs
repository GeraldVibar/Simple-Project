using System;


namespace sari_sari_store_ni_pedro
{
    class Program
    {
        // Constants and global variables
        const int max_items = 50;
        // Maximum number of items the store can hold
        static string[] item_name = new string[max_items];         // Array to store item names
        static decimal[] price_of_items = new decimal[max_items];  // Array to store item prices
        static int[] quantity = new int[max_items];               // Array to store item quantities
        static decimal[] totalprices = new decimal[max_items];     // Array to store total price (price * quantity)

        // Arrays to track sales transactions
        static string[] transactionItems = new string[max_items];        // Store purchased items
        static int[] transactionQuantities = new int[max_items];         // Store purchased quantities
        static decimal[] transactionTotals = new decimal[max_items];     // Store transaction totals
        static int transactionCount = 0;                          // Count of transactions

        // Initial inventory count and variables for product info
        static int count = 6;                          // Starting with 6 pre-loaded products
        static int qty;                                // Variable to store quantity input
        static decimal price;                          // Variable to store price input
        static string name;                            // Variable to store item name input

        // Login credentials for store owner
        static string correctusername = "owner";       // Username for admin access
        static string correctpassword = "password123"; // Password for admin access


        // Adds new product to the inventory after owner authentication

        static void Add_newproduct()
        {
           
            // Login authentication with 3 attempts
            int login_attempt = 3;
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║                     LOGIN                  ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");

            while (login_attempt > 0)
            {
                // Get username and password from user
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                // Validate credentials
                if (username != correctusername && password != correctpassword)
                {
             
                    login_attempt--;
                    Console.WriteLine($"Failed attempts: {login_attempt}");
                    Console.WriteLine("Invalid username or password. Access denied.\n");
                    
                    // Lock access if no more attempts
                    if (login_attempt == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("No more attempts. Access locked.");
                        Console.WriteLine("Press any key to go back to the Main menu...");
                        Console.ReadKey();
                        Console.Clear();


                        return;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Login successful!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                }
            }

            // Product addition loop
            while (true)
            {
                // Check if inventory is full
                if (count < 51)
                {
                    Console.WriteLine("╔════════════════════════════════════════════╗");
                    Console.WriteLine("║              ADD NEW PRODUCT               ║");
                    Console.WriteLine("╚════════════════════════════════════════════╝");

                    // Get product name with duplicate check
                    while (true)
                    {
                        Console.Write("Item name: ");
                        name = Console.ReadLine(); // Keeps original casing

                        bool nameExists = false;

                        // Check if name already exists (case-insensitive)
                        for (int i = 0; i < count; i++)
                        {
                            if (name.ToLower() == item_name[i].ToLower())
                            {
                                Console.WriteLine("Product name already exists. Try again.\n");
                                nameExists = true;
                                break;
                            }
                        }

                        if (!nameExists)
                        {
                            break; // Valid name, exit loop
                        }
                    }

                    // Get valid price (must be positive)
                    Console.Write("Price: ");
                    while (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
                    {
                        Console.Write("Please enter a valid price: ");
                    }

                    // Get valid quantity (must be positive)
                    Console.Write("Quantity: ");
                    while (!int.TryParse(Console.ReadLine(), out qty) || qty < 0)
                    {
                        Console.Write("Please enter a valid quantity:");
                    }

                    // Add new product to inventory arrays
                    item_name[count] = name;
                    price_of_items[count] = price;
                    quantity[count] = qty;
                    count++;

                    Console.Clear();
                    Console.WriteLine("Product added successfully!");

                    // Ask if user wants to add another product
                    Console.Write("Do you want to add another product? (yes/no): ");
                    string response = Console.ReadLine().ToLower();
                    Console.Clear();
                    if (response == "no")
                    {
                        break;
                    }

                    // Validate yes/no response
                    while (response != "yes" && response != "no")
                    {
                        Console.WriteLine("Invalid response. Please enter 'yes' or 'no'.");
                        Console.Write("Do you want to add another product? (yes/no): ");
                        response = Console.ReadLine().ToLower();
                       
                    }
                    Console.Clear();
                }
                else
                {
                    // Inventory limit reached
                    Console.WriteLine("Inventory full! Cannot add more items.");
                    break;
                }
            }
        }


        // Displays current inventory with product details and total value

        static void View_Inventory()
        {
            decimal grandtotal = 0;

            // Check if inventory is empty
            if (count == 0)
            {
                Console.WriteLine("There are no products available to display.\n");
                Console.WriteLine("Press any key to go back to the Main menu...");
                Console.ReadKey();
                Console.Clear();
                return;
             
            }
            else
            {

                Console.WriteLine("╔═════════════════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                                        VIEW INVENTORY                                       ║");
                Console.WriteLine("╚═════════════════════════════════════════════════════════════════════════════════════════════╝");
                // Display inventory table header
                Console.WriteLine($"{"No.",-5} {"Product Name",-20} {"Unit Price",15} {"Quantity",15} {"Total price",18}");
                Console.WriteLine("----------------------------------------------------------------------------------------------");

                // Display each product and calculate grand total
                for (int i = 0; i < count; i++)
                {
                    totalprices[i] = quantity[i] * price_of_items[i];
                    Console.WriteLine($"{i + 1,-5} | {item_name[i],-20} | {price_of_items[i],15} | {quantity[i],15} | {totalprices[i],15}");
                    grandtotal += totalprices[i];
                }
                Console.WriteLine("----------------------------------------------------------------------------------------------");

                // Display grand total value of inventory
                Console.WriteLine($"\t\t\t\t\t\t\t\tGrand Total: {grandtotal}");

                Console.WriteLine("Press any key to go back to the Main menu...");
                Console.ReadKey();
                Console.Clear();

            }
        }


        // Handles product sales and generates receipt

        static void Sell_Product()
        {
            // Arrays to track current purchase session
            string[] purchasedItems = new string[max_items];
            int[] purchasedQuantities = new int[max_items];
            int[] purchasedIndexes = new int[max_items];
            int purchasedCount = 0;
            decimal totalcost = 0, total = 0;
            int choice_item, qty, index;

            if (count == 0)
            {
                Console.WriteLine("There are no products available to sell.\n");
                Console.WriteLine("Press any key to return to the Main menu...");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            while (true)
            {


            
                // Display available products for sale
                    Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║                                        SELL PRODUCT                                        ║");
                    Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════╝");
                    Console.WriteLine($"{"No.",-5} {"Product Name",-20} {" Unit Price",15} {"Quantity",15} {"Total price",18}");
                    Console.WriteLine("----------------------------------------------------------------------------------------------");

                    // List all products
                    for (int i = 0; i < count; i++)
                    {
                        totalprices[i] = quantity[i] * price_of_items[i];
                        Console.WriteLine($"{i + 1,-5} | {item_name[i],-20} | {price_of_items[i],15} | {quantity[i],15} | {totalprices[i],15}");
                    }
                    Console.WriteLine("----------------------------------------------------------------------------------------------");

                    // Get product selection from user
                    Console.Write("Enter the product number you want to buy: ");
                    while (!int.TryParse(Console.ReadLine(), out choice_item) || choice_item <= 0 || choice_item > count)
                    {
                        Console.WriteLine("Invalid item number. please try again.");
                        Console.Write("Enter the product number you want to buy: ");


                    }

                    index = choice_item - 1;

                    // Check if item is in stock
                    if (quantity[index] == 0)
                    {
                    Console.Clear();
                        Console.WriteLine("Item is OUT OF STOCK.");
                        continue;
                    }

                    // Get quantity to purchase
                    Console.Write("How many do you want to buy? ");
                    while (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                    {
                        Console.WriteLine("The quantity you entered is not valid. Please try again.");
                        Console.Write("How many do you want to buy? ");
                    }

                    // Check if enough stock available
                    if (qty > quantity[index])
                    {
                    Console.Clear();
                        Console.WriteLine("Not enough stock.");
                        continue;
                    }

                    Console.Clear();
                    // Update inventory quantity
                    quantity[index] -= qty;
                     Console.WriteLine($"Purchased {qty} x {item_name[index]}. Remaining stock: {quantity[index]}\n");


                // Add to current purchase tracking (combine same items)
                bool found = false;
                    for (int i = 0; i < purchasedCount; i++)
                    {
                        if (purchasedIndexes[i] == index)
                        {
                            purchasedQuantities[i] += qty;
                            found = true;
                            break;
                        }
                    }

                    // Add new item to purchase list
                    if (!found)
                    {
                        purchasedItems[purchasedCount] = item_name[index];
                        purchasedQuantities[purchasedCount] = qty;
                        purchasedIndexes[purchasedCount] = index;
                        purchasedCount++;
                    }

                    // Record transaction for sales history
                    transactionItems[transactionCount] = item_name[index];
                    transactionQuantities[transactionCount] = qty;
                    transactionTotals[transactionCount] = qty * price_of_items[index];
                    transactionCount++;

                    // Ask if user wants to purchase more
                    Console.WriteLine("Do you want to buy again?(yes/no): ");
                    string ans = Console.ReadLine().ToLower();
                   
                while (ans != "yes" && ans != "no")
                {
                    Console.WriteLine("Invalid response. Please enter 'yes' or 'no'.");
                    Console.Write("Do you want to add another product? (yes/no): ");
                    ans = Console.ReadLine().ToLower();

                }
                Console.Clear();
                // Generate receipt when finished
                if (ans == "no")
                    {
                        // Display receipt header
                        Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║                                      OFFICIAL RECEIPT                                      ║");
                        Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════╝");

                        Console.WriteLine($"{"No.",-5} | {"Product Name",-20} | {"Quantity",15} {"Total price",18}");
                        Console.WriteLine("--------------------------------------------------------------------------------------------");

                        // List all purchased items
                        for (int i = 0; i < purchasedCount; i++)
                        {
                            total = purchasedQuantities[i] * price_of_items[purchasedIndexes[i]];
                            totalcost += total;
                            Console.WriteLine($"{i + 1,-5} | {purchasedItems[i],-20} | x{purchasedQuantities[i],15} | {total,10}");
                        }

                        // Display total cost
                        Console.WriteLine("--------------------------------------------------------------------------------------------");
                        Console.WriteLine($"TOTAL COST: {totalcost}\n");
                        Console.WriteLine("Thank you for your purchase!\n");

                        Console.WriteLine("Press any key to go back to the menu...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    } 
                      

            }
               
        }

        // Displays sales history with combined totals by product

        static void View_Sales()
        {
            // Check if any sales have been made
            if (transactionCount == 0)
            {
                Console.WriteLine("No sales made yet.\n");
                Console.WriteLine("Press any key to go back to the menu...");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            // Display sales header
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                          VIEW SALES                                        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine($"{"No.",-5} {"Product Name",-20} {"Quantity Sold",-10} {"Total Cost",15}");
            Console.WriteLine("---------------------------------------------------------------------------------------------");

            // Arrays to store combined sales data (aggregating by product)
            string[] combinedNames = new string[transactionCount];
            int[] combinedQuantities = new int[transactionCount];
            decimal[] combinedTotals = new decimal[transactionCount];
            int combinedCount = 0;

            // Combine sales data for the same product
            for (int i = 0; i < transactionCount; i++)
            {
                bool found = false;

                // Check if product already exists in combined list
                for (int j = 0; j < combinedCount; j++)
                {
                    if (transactionItems[i] == combinedNames[j])
                    {
                        // Update existing product quantities and totals
                        combinedQuantities[j] += transactionQuantities[i];
                        combinedTotals[j] += transactionTotals[i];
                        found = true;
                        break;
                    }
                }

                // Add new product to combined list
                if (!found)
                {
                    combinedNames[combinedCount] = transactionItems[i];
                    combinedQuantities[combinedCount] = transactionQuantities[i];
                    combinedTotals[combinedCount] = transactionTotals[i];
                    combinedCount++;
                }
            }

            // Display combined sales data
            for (int i = 0; i < combinedCount; i++)
            {
                Console.WriteLine($"{i + 1,-5} {combinedNames[i],-20} {combinedQuantities[i],-10} {combinedTotals[i],15}");
            }

            Console.WriteLine("---------------------------------------------------------------------------------------------");

            Console.WriteLine("Press any key to go back to the menu...");
            Console.ReadKey();
            Console.Clear();
        }

      

        // Method to handle confirmation without arguments (void)
        static bool ConfirmUpdate()
        {
            Console.Write("Are you sure you want to proceed with this update? (yes/no): ");
            string response = Console.ReadLine().ToLower();

            while (response != "yes" && response != "no")
            {
                Console.WriteLine("Invalid response. Please enter 'yes' or 'no'.");
                Console.Write("Are you sure you want to proceed with this update? (yes/no): ");
                response = Console.ReadLine().ToLower();
              
            }
            Console.Clear();
            if (response == "no")
            {
                
                Console.WriteLine("Update cancelled.");
                return false;// Don't proceed
            }
            else
            {
                Console.WriteLine("Update confirmed.");
                Console.WriteLine("Product name updated successfully.");

                return true;  // Proceed with the update
              
            }
        }
        // Updates existing product information (name, quantity, or price)
        static void Update_Product()
        {
            while (true)
            {
                // Display update product header
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                                      UPDATE PRODUCT                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════╝");

                // Check if inventory is empty
                if (count == 0)
                {
                    Console.WriteLine("The inventory is empty.");
                    Console.WriteLine("Press any key to go back to the menu...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                // Display inventory for selection
                Console.WriteLine($"{"No.",-5} {"Product Name",-20} {" Unit Price",15} {"Quantity",15} {"Total price",18}");
                Console.WriteLine("----------------------------------------------------------------------------------------------");
                for (int i = 0; i < count; i++)
                {
                    totalprices[i] = quantity[i] * price_of_items[i];
                    Console.WriteLine($"{i + 1,-5} | {item_name[i],-20} | {price_of_items[i],15} | {quantity[i],15} | {totalprices[i],15}");
                }
                Console.WriteLine("----------------------------------------------------------------------------------------------");

                // Get product selection from user
                Console.Write("Enter the product number you want to update: ");
                int itemIndex;
                while (!int.TryParse(Console.ReadLine(), out itemIndex) || itemIndex <= 0 || itemIndex > count)
                {
                    Console.WriteLine("Invalid product number. Please try again.");
                    Console.Write("Enter the product number you want to update: ");


                }



                itemIndex--; // Convert to zero-based index
                Console.WriteLine($"You are updating {item_name[itemIndex]}.");

                // Display update options
                Console.WriteLine("\nWhat would you like to update?");
                Console.WriteLine("1. Update Product Name");
                Console.WriteLine("2. Update Quantity");
                Console.WriteLine("3. Update Price");
                Console.Write("Enter your choice (1/2/3): ");

                // Get valid update choice
                int updateChoice;
                while (!int.TryParse(Console.ReadLine(), out updateChoice) || updateChoice < 1 || updateChoice > 3)
                {
                    Console.Write("Invalid choice. Please enter 1, 2, or 3: ");
                }

                // Process update based on choice
                switch (updateChoice) 
                { 
                    case 1:  // Update name
                    while (true)
                    {
                        Console.Write("Enter the new product name: ");
                        string newProductName = Console.ReadLine();

                        // Check if the new product name already exists in the inventory
                        bool nameExists = false;
                        for (int i = 0; i < count; i++)
                        {
                            // Skip the current item (itemIndex) from being checked
                            if (i != itemIndex && item_name[i].ToLower() == newProductName.ToLower())
                            {
                                nameExists = true;
                                break;
                            }
                        }

                            if (nameExists)
                            {
                                Console.WriteLine("The product name already exists. Please choose a different name.\n");
                                continue;
                            }
                            else
                            {

                                // Confirm update
                                Console.Write("Are you sure you want to update the product name? (yes/no): ");
                                string confirmation = Console.ReadLine().ToLower();
                                while (confirmation != "yes" && confirmation != "no")
                                {
                                    Console.Write("Invalid response. Please enter 'yes' or 'no': ");

                                    confirmation = Console.ReadLine().ToLower();
                                }
                                Console.Clear();

                                if (confirmation == "no")
                                {
                                    break; // Exit case
                                }
                                Console.Clear();

                                if (confirmation == "yes")
                                {
                                    item_name[itemIndex] = newProductName;
                                    Console.WriteLine("Product name updated successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("Update cancelled.");
                                }
                                break;
                            }

                                 
                    }
                        break;









                case 2:
                        // Update quantity
                        Console.Write("Enter the new quantity: ");
                        int newQuantity;
                        while (!int.TryParse(Console.ReadLine(), out newQuantity) || newQuantity < 0)
                        {
                            Console.Write("Invalid quantity. Please enter a valid quantity: ");
                        }
                        if (ConfirmUpdate())  // Ask for confirmation
                        {
                            quantity[itemIndex] = newQuantity;
                        }

                        break;

                    case 3:
                        // Update price
                        Console.Write("Enter the new price: ");
                        decimal newPrice;
                        while (!decimal.TryParse(Console.ReadLine(), out newPrice) || newPrice < 0)
                        {
                            Console.Write("Invalid price. Please enter a valid price: ");
                        }
                        if (ConfirmUpdate())  // Ask for confirmation
                        {
                            price_of_items[itemIndex] = newPrice;
                          
                        }

                        break;

                }

                // Ask if user wants to delete another product
                Console.Write("Do you want to update another product? (yes/no): ");
                string answer = Console.ReadLine().ToLower();
                Console.Clear();
                while (answer != "yes" && answer != "no")
                {
                    Console.Write("Invalid input. Please enter yes or no: ");
                    answer = Console.ReadLine().ToLower();
                }

                if (answer == "no")
                {
                    break;
                }




            }
           
        }


        // Deletes a product from inventory

        static void Delete_Product()
        {
            while (true)
            {
                Console.Clear(); // Clear screen at the beginning of each loop
                                 // Display delete product header
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                                      DELETE PRODUCT                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════╝");
                if (count == 0)
                {
                    Console.WriteLine("The inventory is empty.");
                    Console.WriteLine("Press any key to return to the Main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }


                // Display inventory for selection
                Console.WriteLine($"{"No.",-5} {"Product Name",-20} {" Unit Price",15} {"Quantity",15} {"Total price",18}");
                Console.WriteLine("----------------------------------------------------------------------------------------------");
                for (int i = 0; i < count; i++)
                {
                    totalprices[i] = quantity[i] * price_of_items[i];
                    Console.WriteLine($"{i + 1,-5} | {item_name[i],-20} | {price_of_items[i],15} | {quantity[i],15} | {totalprices[i],15}");
                }
                Console.WriteLine("----------------------------------------------------------------------------------------------");

                // Get product selection from user
                Console.Write("Enter the product number you want to delete: ");
                int productNumber;

                while (!int.TryParse(Console.ReadLine(), out productNumber) || productNumber <= 0 || productNumber > count)
                {
                    Console.WriteLine("Invalid product number. Please try again.");
                    Console.Write("Enter the product number you want to delete: ");
                }
                Console.Write("Are you sure you want to delete this product? (yes/no): ");
                string response = Console.ReadLine().ToLower();
                while (response != "yes" && response != "no")
                {
                    Console.WriteLine("Invalid response. Please enter 'yes' or 'no'.");
                    Console.Write("Are you sure you want to delete this product? (yes/no): ");
                    response = Console.ReadLine().ToLower();
                }
                if (response == "yes")
                {
                    int index = productNumber - 1; // Convert to zero-based index

                    // Shift remaining products to fill the gap
                    for (int i = index; i < count - 1; i++)
                    {
                        item_name[i] = item_name[i + 1];
                        price_of_items[i] = price_of_items[i + 1];
                        quantity[i] = quantity[i + 1];
                        totalprices[i] = totalprices[i + 1];
                    }

                    // Clear the last position
                    item_name[count - 1] = null;
                    price_of_items[count - 1] = 0;
                    quantity[count - 1] = 0;
                    totalprices[count - 1] = 0;

                    // Decrease item count
                    count--;

                    Console.WriteLine("Product deleted successfully!\n");

                }
                else
                {
                    Console.WriteLine("No product was deleted.\n");

                }

                // Ask if user wants to delete another product
                Console.Write("Do you want to delete another product? (yes/no): ");
                string answer = Console.ReadLine().ToLower();

                while (answer != "yes" && answer != "no")
                {
                    Console.Write("Invalid input. Please enter yes or no: ");
                    answer = Console.ReadLine().ToLower();
                }

                if (answer == "no")
                {
                    Console.WriteLine("Press any key to go back to the Main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                }

            }

        }


        // Main program entry point

        static void Main(string[] args)
        {
            // Initialize inventory with default products
            item_name[0] = "Chocolate"; price_of_items[0] = 99.99M; quantity[0] = 10;
            item_name[1] = "Boy Bawang"; price_of_items[1] = 34.50M; quantity[1] = 15;
            item_name[2] = "Cafe Blanca"; price_of_items[2] = 12M; quantity[2] = 20;
            item_name[3] = "Bear Brand"; price_of_items[3] = 11.50M; quantity[3] = 25;
            item_name[4] = "Tender Juicy Hotdog"; price_of_items[4] = 149.75M; quantity[4] = 12;
            item_name[5] = "Yakult"; price_of_items[5] = 10.50M; quantity[5] = 30;

            // Main program loop
            while (true)
            {
                // Display main menu
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    SARI-SARI STORE                     ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Welcome to the store! What would you like to do today? ║");
                Console.WriteLine("║  [1] Add New Product                                   ║");
                Console.WriteLine("║  [2] View Inventory                                    ║");
                Console.WriteLine("║  [3] Sell Products                                     ║");
                Console.WriteLine("║  [4] View Sales                                        ║");
                Console.WriteLine("║  [5] Update Product                                    ║");
                Console.WriteLine("║  [6] Delete Product                                    ║");
                Console.WriteLine("║  [7] Exit                                              ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");

                // Get user menu choice
                Console.Write("Enter your choice (1-7): ");
                if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice >=8)
                {
                    Console.Clear();
                    Console.WriteLine("That's not a valid option. Please choose a number between 1 and 7.");
                    
                    continue;
                }

                // Process menu selection
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Add_newproduct();  // Add new product to inventory
                        break;

                    case 2:
                        Console.Clear();
                        View_Inventory();  // View current inventory
                        Console.WriteLine();
                        break;

                    case 3:
                        Console.Clear();
                        Sell_Product();    // Sell products to customer
                        break;

                    case 4:
                        Console.Clear();
                        View_Sales();      // View sales history
                        break;

                    case 5:
                        Console.Clear();
                        Update_Product();  // Update existing product
                        break;

                    case 6:
                        Console.Clear();
                        Delete_Product();  // Delete product from inventory
                        break;

                    case 7:
                        Console.WriteLine("Thank you for visiting Sari-Sari Store ni Pedro! Have a great day!");
                        return;  // Exit program

                 

                }
            }
        }
    }
}
