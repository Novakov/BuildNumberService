using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using DapperExtensions;

namespace BuildNumberService
{
    public class Operations
    {
        private readonly SQLiteConnection connection;

        public Operations(ConnectionFactory factory)
        {
            this.connection = factory.GetConnection();
        }

        public void CreateDatabase()
        {
            var cmd = this.connection.CreateCommand();
            cmd.CommandText = @"Create table if not exists Numbers 
(
    Key nvarchar(32768) not null, 
    LastVersion int not null default 0, 
    ObtainedAt datetime not null,
    primary key (Key)
)";

            cmd.ExecuteNonQuery();
        }

        public int NextNumber(string key)
        {
            var number = this.connection.Get<Number>(key);

            if (number == null)
            {
                return 0;
            }
            else
            {
                return number.LastVersion + 1;
            }
        }

        public void Confirm(string key, int confirmedNumber)
        {
            var number = this.NextNumber(key);

            if (number != confirmedNumber)
            {
                throw new NumberMismatchException(confirmedNumber, number);
            }

            if (number == 0)
            {
                this.connection.Insert(new Number { Key = key, LastVersion = 0, ObtainedAt = DateTime.Now });
            }
            else
            {
                this.connection.Update<Number>(new Number
                {
                    Key = key,
                    LastVersion = confirmedNumber,
                    ObtainedAt = DateTime.Now
                });
            }
        }

        public IEnumerable<Number> List()
        {
            return this.connection.GetList<Number>(sort: new ISort[] {new Sort() {PropertyName = "Key", Ascending = true}})
                .ToList();
        }
    }

    public class NumberMismatchException : Exception
    {
        public NumberMismatchException(int confirmedNumber, int actualNumber)
            : base(string.Format("Number mismatch. Confirming: {0} Actual: {1}", confirmedNumber, actualNumber))
        {
            
        }
    }
}