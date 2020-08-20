using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Staj.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
