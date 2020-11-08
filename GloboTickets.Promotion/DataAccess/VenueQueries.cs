using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboTickets.Promotion.DataAccess.Entities;
using GloboTickets.Promotion.Models;
using Microsoft.EntityFrameworkCore;

namespace GloboTickets.Promotion.DataAccess
{
    public class VenueQueries
    {
        private readonly PromotionContext repository;

        public VenueQueries(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task<List<VenueModel>> ListVenues()
        {
            var result = await repository.Venue
                .Select(venue => new
                {
                    VenueGuid = venue.VenueGuid,
                    Description = venue.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return result
                .Select(row => MapVenue(row.VenueGuid, row.Description))
                .ToList();
        }

        public async Task<VenueModel> GetVenue(Guid venueGuid)
        {
            var result = await repository.Venue
                .Where(venue => venue.VenueGuid == venueGuid)
                .Select(venue => new
                {
                    VenueGuid = venue.VenueGuid,
                    Description = venue.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault()
                })
                .SingleOrDefaultAsync();

            return result == null ? null : MapVenue(result.VenueGuid, result.Description);
        }

        private VenueModel MapVenue(Guid venueGuid, VenueDescription venueDescription)
        {
            return new VenueModel
            {
                VenueGuid = venueGuid,
                Name = venueDescription?.Name,
                City = venueDescription?.City,
                LastModifiedTicks = venueDescription?.ModifiedDate.Ticks ?? 0
            };
        }
    }
}