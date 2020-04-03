using BinInfo;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Infrastructure.External
{
    /// <summary>
    /// Leverages binlist.net to pull metadata about a provided BIN.
    /// </summary>
    public class BinInfoBinService 
    {
        private readonly ILogger<BinInfoBinService> _logger;

        public BinInfoBinService(ILogger<BinInfoBinService> logger)
        {
            _logger = logger;
        }

        public async Task Check(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number)));
            }

            if (number.Length > 6)
            {
                number = number.Substring(0, 6);
            }


            var info = await BinList.FindAsync(number);
            switch (info.Bank.Name)
            {
                case "ROYAL":
                    break;
                case "CIBC":
                    break;
            }
        }
    }
}
