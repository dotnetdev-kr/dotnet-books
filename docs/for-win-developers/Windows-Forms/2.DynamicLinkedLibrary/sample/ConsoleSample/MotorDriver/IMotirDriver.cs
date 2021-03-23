using System;
using System.Threading.Tasks;

namespace Driver
{
    /// <summary>
    /// MotorDriver 인스턴스를 생성하는 팩토리 인터페이스
    /// </summary>
    public interface IMotorDriverFactory
    {
        string DriverName { get; }

        IMotorDriver Create(int id);
    }

    /// <summary>
    /// MotorDriver 인터페이스
    /// </summary>
    public interface IMotorDriver
    {
        int Id { get; }

        double Position { get; }
        Task MoveAsync(double position);
        Task ResetAsync();
    }
}
