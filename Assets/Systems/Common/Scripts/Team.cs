using System;
using System.Collections.Generic;

[Serializable]
public class Team
{
    public TeamSide side;
    private List<Character> _characters = new();
    public List<Character> Characters => _characters;
    public int PlayerNum => _characters.Count;

    public Team(TeamSide teamSide)
    {
        side = teamSide;
    }

    public void AddCharacter(Character player)
    {
        _characters.Add(player);
    }

    public void SetServe()
    {
        if (_characters.Count > 0)
        {
            _characters[0].SetServe();
        }
    }
}