
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Contracts;
using Contracts.DataManagers;
using Contracts.Entities;
using Contracts.Entities.Data;
using Contracts.Enums;
using Contracts.Exceptions;


namespace Logic.DataManagers
{
    public class TestManager : ITestManager
    {
        public void Test()
        {
            using (var reader = new StreamReader(@"D:\Google Drive\Projects\Forex"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        string[] values = line.Split(',');
                        if (values.Length >= 4)
                        {

                        }
                    }
                }
            }
        }
    }
}