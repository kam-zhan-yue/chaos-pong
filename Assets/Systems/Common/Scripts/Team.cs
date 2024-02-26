using System;
using System.Collections.Generic;

[Serializable]
public class Team
{
    public TeamSide side;
    private List<Character> _characters = new();
    public List<Character> Characters => _characters;
    public int CharacterCount => _characters.Count;

    public int PlayerCount()
    {
        int count = 0;
        for (int i = 0; i < _characters.Count; ++i)
        {
            if (_characters[i].GetType() == typeof(Player))
            {
                count++;
            }
        }
        return count;
    }

    public List<Player> Players
    {
        get
        {
            List<Player> players = new();
            for (int i = 0; i < _characters.Count; ++i)
            {
                if (_characters[i].GetType() == typeof(Player))
                {
                    players.Add((Player)_characters[i]);
                }
            }
            return players;
        }
    }

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
            _characters[0].SetStart();
        }
    }
}