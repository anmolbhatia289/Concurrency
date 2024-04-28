using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharingBathroom
{
    public class Program
    {
        public enum BathroomStatus
        {
            Male,
            Female,
            Neutral
        }

        public enum Gender
        {
            Male,
            Female
        }

        public Program()
        {

        }

        public object bathroom = new object();
        int peopleInBathroom = 0;
        public BathroomStatus bathroomStatus = BathroomStatus.Neutral;
        Semaphore maxMembers = new Semaphore(3, 3);

        public void useBathroom(Gender gender)
        {
            lock (bathroom)
            {
                if (gender == Gender.Male)
                {
                    while (bathroomStatus == BathroomStatus.Female)
                    {
                        Thread.Sleep(1000);
                        Monitor.Wait(bathroom);
                    }
                }
                else
                {
                    while (bathroomStatus == BathroomStatus.Male)
                    {
                        Monitor.Wait(bathroom);
                    }
                }

                maxMembers.WaitOne();
                peopleInBathroom++;
            }

            Console.WriteLine($"{gender.ToString()} using bathroom");
            useBathroom();

            lock (bathroom)
            {
                peopleInBathroom--;
                if (peopleInBathroom == 0)
                {
                    bathroomStatus = BathroomStatus.Neutral;
                }
                Monitor.PulseAll(bathroom);
                maxMembers.Release();
            }
        }

        public void useBathroom()
        {
            Random random = new Random();
            Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(1000,2000)));
        }

        public static void Main(string[] args)
        {
            Program program = new Program();
            Thread maleThread = new Thread(() => program.useBathroom(Gender.Male));
            Thread femaleThread = new Thread(() => program.useBathroom(Gender.Male));
            maleThread.Start();
            femaleThread.Start();
            maleThread.Join();
            femaleThread.Join();
        }
    }
}
