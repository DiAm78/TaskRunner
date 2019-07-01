using System.Threading.Tasks;

namespace TaskRunner.Service
{
    public class Calculator
    {

        public async Task<int> Plus(int a, int b)
        {
            return await Task.Run(() =>
            {
                 return a + b;
            });
        }
        

        public async Task<int> Minus(int a, int b)
        {
            return await Task.Run(() =>
            {
                return a - b;
            });
        }

        public async Task<int> Multiply(int a, int b)
        {
                return await Task.Run(() =>
                {
                    return a * b;
                });
        }

        public async Task<int> Divide(int a, int b)
        {
                    return await Task.Run(() =>
                    {
                        if (b > 0)

                return a / b;
            else
                return a;
                    });
        }
    }
}
