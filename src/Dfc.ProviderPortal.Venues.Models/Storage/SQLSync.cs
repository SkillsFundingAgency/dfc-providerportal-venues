
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Dfc.ProviderPortal.Venues.Models;


namespace Dfc.ProviderPortal.Venues
{
    static public class SQLSync
    {
        static public IEnumerable<Venue> GetAll(out int count)
        {
            using (DbConnection cn = DBHelper.Connection)
            {
                List<Venue> results = new List<Venue>();
                int i = 0;

                try
                {
                    //DbCommand cm = new SqlCommand("up_VenueListForCsvExport", (SqlConnection)cn);
                    DbCommand cm = new SqlCommand("up_UKRLP_Course_Directory_Migration", (SqlConnection)cn);
                    DbDataReader dr = cm.ExecuteReader();
                    while (dr.Read()) {
                        results.Add(
                            new Venue()
                            {
                                UKPRN = dr.GetInt32(dr.GetOrdinal("UKPRN")),
                                PROVIDER_ID = dr.GetInt32(dr.GetOrdinal("PROVIDER_ID")),
                                VENUE_ID = dr.GetInt32(dr.GetOrdinal("VENUE_ID")),
                                VENUE_NAME = dr.SafeGetString("VENUE_NAME"),
                                PROV_VENUE_ID = dr.SafeGetString("PROV_VENUE_ID"),
                                //PHONE = dr.SafeGetString("PHONE"),
                                ADDRESS_1 = dr.SafeGetString("ADDRESS_1"),
                                ADDRESS_2 = dr.SafeGetString("ADDRESS_2"),
                                TOWN = dr.SafeGetString("TOWN"),
                                COUNTY = dr.SafeGetString("COUNTY"),
                                POSTCODE = dr.SafeGetString("POSTCODE"),
                                //EMAIL = dr.SafeGetString("EMAIL"),
                                //WEBSITE = dr.SafeGetString("WEBSITE"),
                                //FAX = dr.SafeGetString("FAX"),
                                //FACILITIES = dr.SafeGetString("FACILITIES"),
                                //DATE_CREATED = dr.SafeGetString("DATE_CREATED"),
                                //DATE_UPDATE = dr.SafeGetString("DATE_UPDATE"),
                                //STATUS = dr.SafeGetString("STATUS"),
                                //UPDATED_BY = dr.SafeGetString("UPDATED_BY"),
                                //CREATED_BY = dr.SafeGetString("CREATED_BY"),
                                //X_COORD = double.IsNaN(dr.SafeGetDouble("X_COORD")) ? "" : dr.SafeGetDouble("X_COORD").ToString(),
                                //Y_COORD = double.IsNaN(dr.SafeGetDouble("Y_COORD")) ? "" : dr.SafeGetDouble("Y_COORD").ToString(),
                                //SEARCH_REGION = dr.SafeGetString("SEARCH_REGION"),
                                //SYS_DATA_SOURCE = dr.SafeGetString("SYS_DATA_SOURCE"),
                                //DATE_UPDATED_COPY_OVER = dr.SafeGetString("DATE_UPDATED_COPY_OVER"),
                                //DATE_CREATED_COPY_OVER = dr.SafeGetString("DATE_CREATED_COPY_OVER")
                            }
                        );
                        i++;
                    }

                    // Clean up and exit
                    dr.Close();
                    cm.Dispose();

                } catch (Exception ex) {
                    throw ex;
                } finally {
                    if (cn.State == ConnectionState.Open)
                        cn.Close();
                    cn.Dispose();
                }
                count = i;
                return results;
            }
        }
    }
}
