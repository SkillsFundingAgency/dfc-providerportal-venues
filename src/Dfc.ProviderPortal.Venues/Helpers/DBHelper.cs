
//using System;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Collections.Generic;


//namespace Dfc.ProviderPortal.Venues
//{
//    static public class DBHelper
//    {
//        static public DbConnection Connection {
//            get {
//                try {
//                    SqlConnection cn = new SqlConnection(SettingsHelper.ConnectionString);
//                    cn.Open();
//                    return cn;
//                }
//                catch (Exception ex) {
//                    throw ex;
//                }
//            }
//        }

//        static public string SafeGetString(this DbDataReader reader, string colName)
//        {
//            int ordinal = reader.GetOrdinal(colName);
//            if (reader.IsDBNull(ordinal))
//                return string.Empty;
//            else
//                return reader.GetString(ordinal).Trim();
//        }

//        static public double SafeGetDouble(this DbDataReader reader, string colName)
//        {
//            int ordinal = reader.GetOrdinal(colName);
//            if (reader.IsDBNull(ordinal))
//                return double.NaN;
//            else
//                return reader.GetDouble(ordinal);
//        }

//        static public int SafeGetInt(this DbDataReader reader, string colName)
//        {
//            int ordinal = reader.GetOrdinal(colName);
//            if (reader.IsDBNull(ordinal))
//                return int.MinValue;
//            else
//                return reader.GetInt32(ordinal);
//        }
//    }
//}
