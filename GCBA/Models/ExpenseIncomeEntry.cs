﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBA.Models
{
    public enum PandLType
    {
        Income, Expenses
    }
    public class ExpenseIncomeEntry
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string AccountName { get; set; }
        public PandLType EntryType { get; set; }
    }
}