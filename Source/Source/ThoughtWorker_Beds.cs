using System;
using System.Linq;
using RimWorld;
using Verse;

namespace Hospitality
{
    /// <summary>
    /// Loaded via xml. Added so guests want beds.
    /// </summary>
    public class ThoughtWorker_Beds : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            try
            {
                if (pawn == null) return ThoughtState.Inactive;
                if (pawn.thingIDNumber == 0) return ThoughtState.Inactive; // What do you know!!!

                if (Current.ProgramState != ProgramState.Playing)
                {
                    return ThoughtState.Inactive;
                }
                if (!pawn.IsArrivedGuest(out var compGuest)) return ThoughtState.Inactive;

                if (compGuest == null) return ThoughtState.Inactive;
                if (!compGuest.arrived) return ThoughtState.Inactive;
                if (compGuest.rescued) return ThoughtState.Inactive;

                var area = pawn.GetGuestArea();

                var bedCount = pawn.MapHeld.GetGuestBeds(pawn.GetGuestArea()).Count(b => b?.def.useHitPoints == true); // Sleeping spots don't count
                if (bedCount == 0) return ThoughtState.ActiveAtStage(0);

                var visitors = pawn.GetMapComponent().presentGuests.Count(p => StaysInArea(p, area));

                if (bedCount < visitors && !compGuest.HasBed) return ThoughtState.ActiveAtStage(1);
                if(bedCount > visitors*1.3f && bedCount > visitors+3) return ThoughtState.ActiveAtStage(3);

                return ThoughtState.ActiveAtStage(2);
            }
            catch(Exception e)
            {
                Log.Warning(e.Message);
                return ThoughtState.Inactive;
            }
        }

        private static bool StaysInArea(Pawn pawn, Area area)
        {
            if (pawn == null) return false;

            var comp = pawn.CompGuest();
            return comp?.arrived == true && comp.GuestArea == area;
        }
    }
}