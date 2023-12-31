﻿using System.Collections.Generic;

using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        void Add(Coffee coffeeBev);
        void Delete(int id);
        Coffee Get(int id);
        List<Coffee> GetAll();
        void Update(Coffee coffeeBev);
    }
}