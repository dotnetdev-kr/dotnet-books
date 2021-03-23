using Driver;

using System;
using System.Threading.Tasks;

namespace BMorterDriver
{
    public class BMotorDriver : IMotorDriver
    {
        public int Id { get; private set; }

        public BMotorDriver(int id)
        {
            this.Id = id;
        }

        public double Position { get; private set; }

        public async Task MoveAsync(double position)
        {
            var sp = this.Position;
            var ep = position;

            for (var i = sp; i <= ep; i++)
            {
                this.Position = i;
                Console.WriteLine($"{i} 퓨웅~");
                await Task.Delay(10);
            }
        }

        public async Task ResetAsync()
        {
            await Task.Yield();
            Position = 0;
        }
    }

    public class AMotorDriverFactory : IMotorDriverFactory
    {
        public string DriverName => "B Motor Driver";

        public IMotorDriver Create(int id) => new BMotorDriver(id);
    }
}
