using Driver;

using System;
using System.Threading.Tasks;

namespace AMotorDriver
{
    public class AMotorDriver : IMotorDriver
    {
        public int Id { get; private set; }

        public AMotorDriver(int id)
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
                Console.WriteLine($"{i} 찌잉~");
                await Task.Delay(100);
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
        public string DriverName => "A Motor Driver";

        public IMotorDriver Create(int id) => new AMotorDriver(id);
    }
}
