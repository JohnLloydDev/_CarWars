using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;


public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance { get; private set; }

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();
    private new PhotonView photonView;

    public TMP_Text scoreText;
    public TMP_Text resultText;
    public TMP_Text countdownText;

    public float countdownTime = 60f; 
    private float currentTime;
    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            photonView = GetComponent<PhotonView>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeScores();
        FindScoreText();
        UpdateScoreUI();
        currentTime = countdownTime;
    }

    private void Update()
    {
        if (!gameEnded)
        {
            currentTime -= Time.deltaTime;

            if (countdownText != null)
            {
                countdownText.text = $"Time Left: {Mathf.Ceil(currentTime)}s";
            }

            if (currentTime <= 0)
            {
                DetermineWinner();
            }
        }
    }

    private void InitializeScores()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!playerScores.ContainsKey(player.ActorNumber))
            {
                playerScores[player.ActorNumber] = 0;
            }
        }
    }

    private void FindScoreText()
    {
        if (scoreText == null)
        {
            GameObject textObj = GameObject.Find("ScoreText");
            if (textObj != null)
            {
                scoreText = textObj.GetComponent<TMP_Text>();
            }
        }
    }

    [PunRPC]
    public void AddScoreRPC(int actorNumber, int points)
    {
        if (playerScores.ContainsKey(actorNumber))
        {
            playerScores[actorNumber] += points;
        }
        else
        {
            playerScores[actorNumber] = points;
        }

        UpdateScoreUI();

        if (playerScores[actorNumber] >= 10)
        {
            photonView.RPC("DeclareWinner", RpcTarget.All, actorNumber);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            string scoreDisplay = "Scores\n";
            foreach (var entry in playerScores)
            {
                Player player = PhotonNetwork.CurrentRoom?.GetPlayer(entry.Key);
                string playerName = (player != null && !string.IsNullOrEmpty(player.NickName)) ? player.NickName : $"Player {entry.Key}";
                scoreDisplay += $"{playerName}: {entry.Value}\n";
            }
            scoreText.text = scoreDisplay;
        }
    }

    private void DetermineWinner()
    {
        if (gameEnded) return;

        gameEnded = true;

        var sortedScores = playerScores.OrderByDescending(entry => entry.Value).ToList();

        int highestScore = sortedScores.FirstOrDefault().Value;

        var topPlayers = sortedScores.Where(entry => entry.Value == highestScore).ToList();

        if (topPlayers.Count > 1)
        {
            string winners = "It's a draw! Top players:\n";
            winners += string.Join("\n", topPlayers.Select(entry =>
            {
                Player player = PhotonNetwork.CurrentRoom?.GetPlayer(entry.Key);
                return (player != null && !string.IsNullOrEmpty(player.NickName)) ? player.NickName : $"Player {entry.Key}";
            }));
            resultText.text = winners;
        }
        else
        {
            Player winner = PhotonNetwork.CurrentRoom?.GetPlayer(topPlayers[0].Key);
            string winnerName = (winner != null && !string.IsNullOrEmpty(winner.NickName)) ? winner.NickName : $"Player {topPlayers[0].Key}";
            resultText.text = $"Winner: {winnerName}!";
        }

        string gameResult = "Game Result:\n";
        foreach (var entry in sortedScores)
        {
            Player player = PhotonNetwork.CurrentRoom?.GetPlayer(entry.Key);
            string playerName = (player != null && !string.IsNullOrEmpty(player.NickName)) ? player.NickName : $"Player {entry.Key}";
            gameResult += $"{playerName}: {entry.Value}\n";
        }

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties["GameResult"] = gameResult;
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

        Invoke(nameof(LoadNextScene), 3f);
    }



    private void LoadNextScene()
    {
        SceneManager.LoadScene("GameResult"); 
    }

    [PunRPC]
    private void DeclareWinner(int actorNumber)
    {
        if (gameEnded) return;

        gameEnded = true;
        Player winner = PhotonNetwork.CurrentRoom?.GetPlayer(actorNumber);
        resultText.text = $"Winner: {winner?.NickName}";
    }
}
