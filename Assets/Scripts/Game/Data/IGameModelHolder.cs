namespace Game.Data
{
    public interface IGameModelHolder
    {
        void SetModel(GameModel model);
        bool HasModel();
        SerializableGameModel GetSerializableGameModel();
    }
}