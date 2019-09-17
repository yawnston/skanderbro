using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Commands;
using Skanderbro.Constants;
using Skanderbro.Models;

namespace Skanderbro.TypeReaders
{
    public sealed class LeaderPipModifiersTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            string[] tokens = input.Split(LeaderConstants.LeaderPipModifiersCommandDelimiter, StringSplitOptions.RemoveEmptyEntries);
            var leaderPipModifiers = new LeaderPipModifiers();

            foreach (string token in tokens)
            {
                if (!TryParseToken(token, leaderPipModifiers))
                {
                    return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, $"Could not parse pip modifier from token {token}."));
                }
            }

            return Task.FromResult(TypeReaderResult.FromSuccess(leaderPipModifiers));
        }

        private bool TryParseToken(string token, LeaderPipModifiers leaderPipModifiers)
        {
            const string tokenRegex = "(fire|shock|maneuver|siege)=(\\d)";

            Match match = Regex.Match(token, tokenRegex);
            if (!match.Success)
            {
                return false;
            }
            if (!int.TryParse(match.Groups[2].Value, out int pipAmount))
            {
                return false;
            }

            switch (match.Groups[1].Value)
            {
                case "fire":
                    leaderPipModifiers.BonusFire = pipAmount;
                    break;
                case "shock":
                    leaderPipModifiers.BonusShock = pipAmount;
                    break;
                case "maneuver":
                    leaderPipModifiers.BonusManeuver = pipAmount;
                    break;
                case "siege":
                    leaderPipModifiers.BonusSiege = pipAmount;
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}
