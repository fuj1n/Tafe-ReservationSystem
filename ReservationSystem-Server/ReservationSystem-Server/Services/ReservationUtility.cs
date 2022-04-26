using JetBrains.Annotations;
using ReservationSystem_Server.Models;

namespace ReservationSystem_Server.Services;

[PublicAPI]
public class ReservationUtility
{
    /**
     * Gets all time slots between <paramref name="startTime"/> and <paramref name="endTime"/>
     * in <paramref name="slotLength"/> increments
     */
    public List<DateTime> GetTimeSlots(DateTime startTime, DateTime endTime, TimeSpan slotLength)
    {
        List<DateTime> timeSlots = new();

        TimeSpan sittingDuration = endTime - startTime;

        //TODO: configurable time slot length
        for (TimeSpan time = new(0); time < sittingDuration; time += slotLength)
        {
            timeSlots.Add(startTime + time);
        }

        return timeSlots;
    }
}