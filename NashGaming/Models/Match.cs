﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NashGaming.Models
{
    public class Match : IComparable
    {
        [Key]
        public int MatchID { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public virtual Team Team1 { get; set; }
        [Required]
        public virtual Team Team2 { get; set; }
        public string Result { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public League League { get; set; }

        public int CompareTo(object obj)
        {
            Match m = obj as Match;
            return -1*( this.Date.CompareTo(m.Date));
        }
    }
}