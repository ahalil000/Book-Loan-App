using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace BookLoanWindowsServiceApp
{
    public partial class BookLoanService : ServiceBase
    {
        private int eventId = 1;
        public BookLoanService()
        {
            InitializeComponent();

            bookLoanEventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("BookLoanSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                "BookLoanSource",
                "BookLoanNewLog");
            }
            bookLoanEventLog.Source = "BookLoanSource";
            bookLoanEventLog.Log = "BookLoanNewLog";
        }

        protected override void OnStart(string[] args)
        {
            bookLoanEventLog.WriteEntry("Book Loan Service Started.");

            Timer timer = new Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            bookLoanEventLog.WriteEntry("Book Loan Service Stopped.");
        }

        protected override void OnPause()
        {
            bookLoanEventLog.WriteEntry("Book Loan Service Paused.");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            bookLoanEventLog.WriteEntry("Timer Polling Event Triggered.", EventLogEntryType.Information, eventId++);
        }
    }
}
