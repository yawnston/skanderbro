using Skanderbro.Models;
using Skanderbro.Models.Enums;

namespace Skanderbro.Services
{
    public interface ILeaderPipService
    {
        LeaderPipResult CalculateAverageLeaderPips(double tradition, LeaderType leaderType);
        LeaderPipResult CalculateAverageRulerLeaderPips(double tradition, int militarySkill);
    }
}