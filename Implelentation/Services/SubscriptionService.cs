﻿using aqay_apis.Context;
using aqay_apis.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aqay_apis.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionService(ApplicationDbContext context)
        {
            _context = context;
        }
        // List all Plans
        public async Task<ICollection<Plan>> GetAllPlansAsync()
        {
            return await _context.Plans.ToListAsync();
        }
        // List Plan details
        public async Task<Plan> GetPlanByIdAsync(int planId)
        {
            return await _context.Plans.FindAsync(planId);
        }

        // Subscripe to a plan
        public async Task SubscribeAsync(string userId, int planId)
        {
            // Retrieve merchant by ID and check if owner
            var merchant = await _context.Merchants
                                 .Include(u => u.Subscription)
                                 .FirstOrDefaultAsync(u => u.Id == userId);

            if (merchant == null)
            {
                throw new Exception("Merchant not found.");
            }
            if (!merchant.IsOwner)
            {
                throw new Exception("Merchant not an Owner.");
            }           

            // Update merchant subscription status
            merchant.IsSubscriped = true;

            // Add subscription and update merchant in a single transaction
            var sub = await CreateAsync(planId);


            merchant.Subscription = sub;
            merchant.SubscriptionId = sub.Id;
            _context.Merchants.Update(merchant);

            await _context.SaveChangesAsync();
        }

        public async Task<Subscription> CreateAsync(int planId)
        {
            var plan = await _context.Plans.FindAsync(planId);
            if (plan == null)
            {
                throw new Exception("Plan not found.");
            }
            // Create new subscription
            var subscription = new Subscription
            {
                StartDate = DateTime.Now,
                EndDate = plan.Name switch
                {
                    "Monthly" => DateTime.Now.AddMonths(1),
                    "Quarterly" => DateTime.Now.AddMonths(3),
                    "Yearly" => DateTime.Now.AddYears(1),
                    _ => throw new Exception("Invalid plan type.")
                },
                PlanId = plan.Id,
                Merchant = null
            };
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }
        
    }
}
