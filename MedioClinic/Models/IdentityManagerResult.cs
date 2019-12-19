using System;
using System.Collections.Generic;

namespace MedioClinic.Models
{
    public class IdentityManagerResult<TResultState>
        where TResultState : Enum
    {
        private List<string> _errors = new List<string>();

        public bool Success { get; set; }

        public List<string> Errors
        {
            get => _errors;
            set => _errors = value;
        }

        public TResultState ResultState { get; set; }
    }

    public class IdentityManagerResult<TResultState, TData> : IdentityManagerResult<TResultState>
        where TResultState : Enum
    {
        public TData Data { get; set; }
    }
}