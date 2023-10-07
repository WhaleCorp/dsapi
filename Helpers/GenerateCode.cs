using System;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace dsapi.Helpers
{
	public static class GenerateCode
	{
        public static string GetMonitorCode()
        {
            int len = 10;
            StringBuilder code = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i <= len; i++)
            {
                code.Append(GetRandomCharacter(random));
            }
            
            return code.ToString();

            char GetRandomCharacter(Random rnd)
            {
                string text = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
                int index = rnd.Next(text.Length);
                return text[index];
            }
        }
    }
}

