using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiHealth.Models;

namespace WebApiHealth.Data
{
    public class DummyData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<HealthContext>();
                context.Database.EnsureCreated();
                //context.Database.Migrate();

                // Look for any ailments
                if (context.Ailments != null && context.Ailments.Any())
                    return;   // DB has already been seeded

                var ailments = DummyData.GetAilments().ToArray();
                context.Ailments.AddRange(ailments);
                context.SaveChanges();

                var medications = DummyData.GetMedications().ToArray();
                context.Medications.AddRange(medications);
                context.SaveChanges();

                var patients = DummyData.GetPatients(context).ToArray();
                context.Patients.AddRange(patients);
                context.SaveChanges();

                var parties = DummyData.GetParties().ToArray();
                context.Parties.AddRange(parties);
                context.SaveChanges();
            }
        }

        public static List<Ailment> GetAilments()
        {
            List<Ailment> ailments = new List<Ailment>()
            {
                new Ailment {Name="Head Ache"},
                new Ailment {Name="Tummy Pain"},
                new Ailment {Name="Flu"},
                new Ailment {Name="Cold"}
            };
            return ailments;
        }

        public static List<Medication> GetMedications()
        {
            List<Medication> medications = new List<Medication>()
            {
                new Medication {Name="Tylenol", Doses = "2"},
                new Medication {Name="Asprin", Doses = "4"},
                new Medication {Name="Advil", Doses = "3"},
                new Medication {Name="Robaxin", Doses = "2"},
                new Medication {Name="Voltaren", Doses = "1"},
            };
            return medications;
        }

        public static List<Patient> GetPatients(HealthContext db)
        {
            List<Patient> patients = new List<Patient>()
            {

                new Patient
                {
                    Name = "Jim Jones",
                    Ailments = new List<Ailment>(db.Ailments.Take(2)),
                    Medications = new List<Medication>(db.Medications.Take(2))
                },
                new Patient
                {
                    Name = "Ann Smith",
                    Ailments = new List<Ailment>(db.Ailments.Take(1)),
                    Medications = new List<Medication>(db.Medications.OrderBy(m => m.Name).Skip(1).Take(1))
                },
                new Patient
                {
                    Name = "Tom Myers",
                    Ailments = new List<Ailment>(db.Ailments.OrderBy(m => m.Name).Skip(2).Take(2)),
                    Medications = new List<Medication>(db.Medications.OrderBy(m => m.Name).Skip(2).Take(2))
                }
            };
            return patients;
        }

        public static List<Party> GetParties()
        {
            return new List<Party>()
            {
                new Party
                {
                    PartyName="Christmas Party",
                    PartyDate= new DateTime(2019,2,27),
                    ExpectedNumberOfGuests = 5,
                    Location = "Mariot Hotel"
                },
                new Party
                {
                    PartyName="Halloween Party",
                    PartyDate= new DateTime(2019,4,15),
                    ExpectedNumberOfGuests = 10,
                    Location = "Sheraton Wall Center"
                },
                new Party
                {
                    PartyName="Birthday Party",
                    PartyDate= new DateTime(2019,1,31),
                    ExpectedNumberOfGuests = 20,
                    Location = "West Inn Hotel"
                },
                new Party
                {
                    PartyName="New Year Eve Party",
                    PartyDate= new DateTime(2018,12,31),
                    ExpectedNumberOfGuests = 30,
                    Location = "Premier Inn Hotel"
                },
                new Party
                {
                    PartyName="Stag Night Part",
                    PartyDate= new DateTime(2019,3,11),
                    ExpectedNumberOfGuests = 40,
                    Location = "Local Community Center"
                }
            };
        }
    }

}
