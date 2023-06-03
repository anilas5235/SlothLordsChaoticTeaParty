namespace Project.Scripts.Menu
{
    /// <summary>
    ///   <para>All interactable Objects must a script that implements this interface</para>
    /// </summary>
    public interface IInteractable
    {
        public void Interact();

        public void Highlight();
    }
}
