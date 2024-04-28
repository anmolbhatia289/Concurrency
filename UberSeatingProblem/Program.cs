using System;
using System.Threading;

class Demonstration
{
    static void Main()
    {
        new UberSeatingProblemTest().run();
    }
}

public class UberSeatingProblem
{
    private int democratCount;
    private int republicanCount;
    private int rideCount;
    private object padlock;
    private System.Threading.Barrier barrier;
    private Semaphore democratsWaiting;
    private Semaphore republicansWaiting;

    public UberSeatingProblem()
    {
        democratCount = 0;
        republicanCount = 0;
        rideCount = 0;
        barrier = new System.Threading.Barrier(4);
        padlock = new object();
        democratsWaiting = new Semaphore(0, 99999);
        republicansWaiting = new Semaphore(0, 99999);
    }

    public void drive()
    {
        rideCount++;
        Console.WriteLine(String.Format("Uber ride # {0} filled and on its way", rideCount));
    }

    public void seated(String party)
    {
        Console.WriteLine(String.Format("\n{0} {1} seated", party, Thread.CurrentThread.ManagedThreadId));
    }

    public void seatDemocrat()
    {
        bool rideLeader = false;

        Monitor.Enter(padlock);

        democratCount++;
        if (democratCount == 4)
        {
            // release 3 democrat threads to ride along
            democratsWaiting.Release(3);
            rideLeader = true;
            democratCount -= 4;
        }
        else if (democratCount == 2 && republicanCount >= 2)
        {
            // release 1 democrat and 2 republican threads
            democratsWaiting.Release();
            republicansWaiting.Release(2);
            rideLeader = true;

            democratCount -= 2;
            republicanCount -= 2;
        }
        else
        {
            // can't form a valid combination
            Monitor.Exit(padlock);
            democratsWaiting.WaitOne();
        }

        seated("Democrat");
        barrier.SignalAndWait();

        if (rideLeader == true)
        {
            drive();
            Monitor.Exit(padlock);
        }
    }

    public void seatRepublican()
    {
        bool rideLeader = false;

        Monitor.Enter(padlock);

        republicanCount++;
        if (republicanCount == 4)
        {
            // release 3 republican threads to ride along
            republicansWaiting.Release(3);
            rideLeader = true;
            republicanCount -= 4;
        }
        else if (republicanCount == 2 && democratCount >= 2)
        {
            // release 1 republican and 2 democrat threads
            republicansWaiting.Release();
            democratsWaiting.Release(2);
            rideLeader = true;

            republicanCount -= 2;
            democratCount -= 2;
        }
        else
        {
            // can't form a valid combination
            Monitor.Exit(padlock);
            republicansWaiting.WaitOne();
        }

        seated("Republican");
        barrier.SignalAndWait();

        if (rideLeader == true)
        {
            drive();
            Monitor.Exit(padlock);
        }
    }
}

public class UberSeatingProblemTest
{
    private Random random = new Random();

    public void run()
    {
        controlledSimulation();
        // randomSimulation();
    }

    public void randomSimulation()
    {
        UberSeatingProblem problem = new UberSeatingProblem();
        int dems = 0;
        int repubs = 0;

        Thread[] riders = new Thread[16];
        for (int i = 0; i < 16; i++)
        {
            int toss = random.Next(0, 2);

            if (toss == 1)
            {
                riders[i] = new Thread(new ThreadStart(problem.seatDemocrat));
                dems++;
            }
            else
            {
                riders[i] = new Thread(new ThreadStart(problem.seatRepublican));
                repubs++;
            }
        }

        Console.WriteLine(String.Format("Total {0} dems and {1} repubs", dems, repubs));

        for (int i = 0; i < 16; i++)
        {
            riders[i].Start();
        }

        for (int i = 0; i < 16; i++)
        {
            riders[i].Join();
        }
    }


    public void controlledSimulation()
    {
        UberSeatingProblem problem = new UberSeatingProblem();
        int dems = 10;
        int repubs = 10;

        int total = dems + repubs;
        Console.WriteLine(String.Format("Total {0} dems and {1} repubs\n", dems, repubs));

        Thread[] riders = new Thread[total];

        while (total != 0)
        {
            int toss = random.Next(0, 2);

            if (toss == 1 && dems != 0)
            {
                riders[20 - total] = new Thread(new ThreadStart(problem.seatDemocrat));
                dems--;
                total--;
            }
            else if (toss == 0 && repubs != 0)
            {
                riders[20 - total] = new Thread(new ThreadStart(problem.seatRepublican));
                repubs--;
                total--;
            }
        }

        for (int i = 0; i < riders.Length; i++)
        {
            riders[i].Start();
        }

        for (int i = 0; i < riders.Length; i++)
        {
            riders[i].Join();
        }
    }
}