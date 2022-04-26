using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Helpers
{
    public class UserProfileParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1; //Default
        private int _pageSize =10;

        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        } 
    }
}