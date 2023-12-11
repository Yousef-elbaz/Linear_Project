using System;
using System.Net;

class Matrix
{
    private void Welcome()
    {
        //clears console screen 
        Console.Clear();
        // sets the title of the console window 
        Console.Title = "My Calculator App";
        // set the welcome message
        Console.ForegroundColor = ConsoleColor.DarkGray;

        Console.WriteLine("\n\n-------------->>> Welcome To My Calculator App <<<--------------\n\n");
        // sets the text color or forground color 
        Console.ForegroundColor = ConsoleColor.DarkYellow;
    }
    public void Run()
    {
        double[,] A; double[] b; int s;

        //Welcome MSG
        Welcome();

        // Get the matrix and size
        GetMatrix(out A, out b, out s);

        // Display the matrix and vector
        Console.ForegroundColor = ConsoleColor.Green; // Set the text color to Green
        Console.WriteLine("--------------------->>>Your Matrix Is <<<-------------------------");
        Display(A, b);

        // Check if the matrix is a zero matrix
        if (IsZeroMatrix(A, s))
        {
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.Blue; // Set the text color to Blue
            Console.WriteLine("<<<<<<<<<<<-----The matrix is a zero matrix.----->>>>>>>>>>>>>>");
            Console.ResetColor();
            return;
        }

        // swapping row if needed
        SwapRowsIfNeeded(A, b, s);


        //Choose Gaussian OR Reduced
        char ChooseForm = ChooseGaussianOrReduced();
        if (ChooseForm == 'G' || ChooseForm == 'g')
        {
            // Perform Gaussian elimination
            RowOpGaussian(A, b, s);
        }
        else if (ChooseForm == 'R' || ChooseForm == 'r')
        {
            // Perform row reduction
            RowOpReduced(A, b, s);
        }

        // Perform back-substitution
        Elimination(A, b, s);
    }
    private char ChooseGaussianOrReduced()
    {
        char choice;
        do
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Choose Gaussian OR Reduced echelon form (G/R)");
            choice = char.Parse(Console.ReadLine());
            Console.ResetColor();

            if (choice != 'G' && choice != 'g' && choice != 'R' && choice != 'r')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input! Please choose 'G' or 'R'.");
                Console.ResetColor();
            }
        } while (choice != 'G' && choice != 'g' && choice != 'R' && choice != 'r');

        return choice;
    }
    public static void RunMatrixCalculator()
    {
        bool restart = true;
        while (restart)
        {
            Matrix calculator = new Matrix();
            calculator.Run();
            Console.ResetColor();
            Console.WriteLine("Do you want to perform another calculation? (y/n)");
            string input = Console.ReadLine();
            if (input.ToLower() == "n")
            {
                restart = false;
            }
        }
    }
    private void Display(double[,] A, double[] b)
    {
        int s = Convert.ToInt32(Math.Sqrt(A.Length));
        for (int row = 0; row < s; row++)
        {
            for (int col = 0; col < s; col++)
            {
                Console.Write($"\t{A[row, col]}\t ");
            }
            Console.Write($"\t| {b[row]}");
            Console.WriteLine();
        }
        Console.ResetColor();
        Console.WriteLine("-------------------------------------------------------------------");
    }
    private void GetMatrix(out double[,] A, out double[] b, out int s)
    {
        Console.Write("Enter size Of Your (Square) Matrix: ");
        s = int.Parse(Console.ReadLine());

        A = new double[s, s];
        b = new double[s];

        for (int row = 0; row < s; row++)
        {
            for (int col = 0; col < s; col++)
            {
                Console.Write($"A [{row},{col}] = ");
                A[row, col] = double.Parse(Console.ReadLine());
            }

            Console.Write($"(b) {row}: ");
            b[row] = double.Parse(Console.ReadLine());
            Console.WriteLine();
        }
    }
    private bool IsZeroMatrix(double[,] A, int s)
    {
        for (int row = 0; row < s; row++)
        {
            for (int col = 0; col < s; col++)
            {
                if (A[row, col] != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
    private void SwapRowsIfNeeded(double[,] A, double[] b, int s)
    {
        for (int p = 0; p < s; p++)
        {
            if (A[p, p] == 0)
            {
                // Find the first row with a non-zero element in the p-th column
                int swapRow = -1;
                for (int i = p + 1; i < s; i++)
                {
                    if (A[i, p] != 0)
                    {
                        swapRow = i;
                        break;
                    }
                }

                if (swapRow != -1)
                {
                    // Swap the current row (p) with the found row (swapRow)
                    for (int col = 0; col < s; col++)
                    {
                        double swapp_A = A[p, col];
                        A[p, col] = A[swapRow, col];
                        A[swapRow, col] = swapp_A;
                    }
                    double swapp_b = b[p];
                    b[p] = b[swapRow];
                    b[swapRow] = swapp_b;
                }
                else
                {
                    //  when there is no non-zero element is found for swapping
                    Console.WriteLine("No non-zero element found for swapping.");
                    break; // Exit the loop
                }
            }
        }
    }
    private void RowOpGaussian(double[,] A, double[] b, int s)
    {

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>This Is Gaussian Form<<<<<<<<<<<<<<<<<<<<<<< \n");
        Console.ResetColor();
        if (A[s - 1, s - 1] != 0)
        {
            Console.WriteLine("System is Consistent and have one solution \n ");
            for (int p = 0; p < s; p++)
            {
                // GAUSSUIAN METHOD
                // اخر رقم =! zero   consistent one solution
                for (int row = p + 1; row < s; row++)
                {
                    double m = A[row, p] / A[p, p];

                    for (int col = 0; col < s; col++)
                    {
                        A[row, col] = A[row, col] - (m * A[p, col]); // Row operation
                    }

                    b[row] = b[row] - (m * b[p]);
                }
                if (p < s - 1)
                {
                    Display(A, b);
                }
            }
        }
        else
        {     //last p=0 then infinte 
            if (b[s - 1] == 0)
            {
                Console.WriteLine("System is Consistent and have infinitly many solutions \n ");
            }
            // no solution
            else
            {
                Console.WriteLine("System is Inconsistent and have NO solution \n ");
            }
        }

    }
    private void Elimination(double[,] A, double[] b, int s)
    {
        double temp = 0; // Temporary
        double u = 0;    // Unknown
        double[] x = new double[s];

        for (int row = s - 1; row >= 0; row--)
        {
            for (int col = s - 1; col >= 0; col--)
            {
                if (row == col)
                {
                    u = A[row, col];
                    break;
                }
                else
                {
                    temp += A[row, col] * b[col];
                }
            }

            b[row] = (b[row] - temp) / u;
            x[row] = b[row];
            temp = 0;
        }
        if (A[s - 1, s - 1] != 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan; // Set the text color to Cyan
            Console.WriteLine("\n<<<<<<<<<<<<<<<<<<<ANSWER>>>>>>>>>>>>>>>>>>>>\n");
            Console.ForegroundColor = ConsoleColor.Magenta; // Set the text color to Magnetta
            for (int i = 0; i < s; i++)
            {
                Console.WriteLine($"\tX[{i}] = {x[i]}");
            }
            Console.WriteLine("");
        }
    }
    private void RowOpReduced(double[,] A, double[] b, int s)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>This Is Reduced Echlon Form<<<<<<<<<<<<<<<<<<<<<<< \n");
        Console.ResetColor();
        // Perform row reduction operations to achieve reduced row-echelon form
        /*  اقسم الصف اللي فوق علي اول عنصر 
         *  بالتالي اول قيمة هتبقي ب واحد 
         * ( وبعد كده اكمل : اللي (تحت) - ( اللي فوق مضروب فاللي تحت 
            واللي فوق كده كده بواحد
            فبالتالي القيمه اللي انا فيها هتبقي بصفر 
           لانه يعتبر بطرحها من نفسها لما بضربها ف واحد */
        int r = s - 1;
        if (A[s - 1, s - 1] != 0)
        {
            Console.WriteLine("System is Consistent and have one solution \n ");
            for (int p = 0; p < s; p++)
              {
                  double pivot = A[p, p];
                  for (int col = p; col < s; col++)
                  {
                      A[p, col] /= pivot;
                  }
                  b[p] /= pivot;
             
             
             
             
                  for (int row = 0; row < s; row++)
                  {
                      if (row != p)
                      {
                          double factor = A[row, p];
                          for (int col = p; col < s; col++)
                          {
                              A[row, col] -= factor * A[p, col];
                          }
                          b[row] -= factor * b[p];
                      }
                  }
                  Display(A, b);
              }
        }
        else
        {     //last p=0 then infinte 
            if (b[s - 1] == 0)
            {
                Console.WriteLine("System is Consistent and have infinitly many solutions \n ");
            }
            // no solution
            else
            {
                Console.WriteLine("System is Inconsistent and have NO solution \n ");
            }
        }
    }
}


