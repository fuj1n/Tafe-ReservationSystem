using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem_Server.Helper;

public static class ModalHelper
{
    /// <summary>
    /// Returns a <see cref="ContentResult"/> containing a tag to close the modal
    /// </summary>
    public static IActionResult CloseModal(this Controller controller)
    {
        return controller.Content("<modal-close></modal-close>");
    }
    
    /// <summary>
    /// Returns a <see cref="ContentResult"/> containing a tag to close the modal and a tag to refresh the page
    /// </summary>
    public static IActionResult CloseModalAndRefresh(this Controller controller)
    {
        return controller.Content("<modal-close></modal-close><modal-refresh-on-close></modal-refresh-on-close>");
    }
}