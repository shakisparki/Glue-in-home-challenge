using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TT.Deliveries.Web.Api.Extensions
{
    public static class Banner
    {
        public static void WriteBanner()
        {
            string banner = @"   ______                             __          __
  / ____/___  ____  ____ __________ _/ /______   / /
 / /   / __ \/ __ \/ __ `/ ___/ __ `/ __/ ___/  / / 
/ /___/ /_/ / / / / /_/ / /  / /_/ / /_(__  )  /_/  
\__________/_/ /_/\__, /_/ __\__,_/\__/____/  (_)   
   / __ \(_)___  ////_/   / /   (_)_______  ____    
  / / / / / __ \/ __ `/  / /   / / ___/ _ \/ __ \   
 / /_/ / / / / / /_/ /  / /___/ / /  /  __/ / / /   
/_____/_/_/ /_/\__, /  /_____/_/_/   \___/_/ /_/    
              /____/                                ";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(banner);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nBy Shakirudeen Lasisi - For Glue Home");
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
