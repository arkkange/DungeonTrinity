using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private const string _gameName = "Dungeon Trinity";

    private bool _isRefreshingHostList = false;
    private HostData[] _hostList;
    private int _numberOfPlayer = 0;

    private GameObject _playerPrefab;
    private string _mapName = null;


    [SerializeField]
    private GameObject _warriorPrefab;
    [SerializeField]
    private GameObject _priestPrefab;
    [SerializeField]
    private GameObject _archerPrefab;
    [SerializeField]
    private GameObject _canvasPlayers;
    [SerializeField]
    private GameObject _canvasLobby;


    private bool _playerChoose = false;
    private bool _mapChoose = false;
    private bool _isReadytoLaunch = false;
    private bool _isReadytoInstanciate = false;

    private GameObject _buttonWarriorG;
    private GameObject _buttonPriestG;
    private GameObject _buttonArcherG;
    private GameObject _buttonMap1G;
    private GameObject _buttonMap2G;
    private GameObject _buttonMap3G;
    private GameObject _buttonValG;
    private GameObject _buttonServerG;
    private GameObject _buttonSearchG;

    private GameObject _lobbyName;
    private GameObject _player1name;
    private GameObject _player2name;
    private GameObject _player3name;
    private GameObject _buttonValLobby;



    /*********************************************************************\
    |   Start : Initialisation								       		   |
    \*********************************************************************/

    void Start()
    {
        RefreshHostList();

        //Boutons pour Choisir son joueur
        _buttonWarriorG = GameObject.Find("ButtonWarrior");
        Button buttonWarrior = _buttonWarriorG.GetComponent<Button>();
        buttonWarrior.onClick.AddListener(() => { setUpCharacter(1); });

        _buttonPriestG = GameObject.Find("ButtonPriest");
        Button buttonPriest = _buttonPriestG.GetComponent<Button>();
        buttonPriest.onClick.AddListener(() => { setUpCharacter(2); });

        _buttonArcherG = GameObject.Find("ButtonArcher");
        Button buttonArcher = _buttonArcherG.GetComponent<Button>();
        buttonArcher.onClick.AddListener(() => { setUpCharacter(3); });




        //Boutons pour Choisir une Map
        _buttonMap1G = GameObject.Find("ButtonMap1");
        Button buttonMap1 = _buttonMap1G.GetComponent<Button>();
        buttonMap1.onClick.AddListener(() => { setUpMap(1); });

        _buttonMap2G = GameObject.Find("ButtonMap2");
        Button buttonMap2 = _buttonMap2G.GetComponent<Button>();
        buttonMap2.onClick.AddListener(() => { setUpMap(2); });

        _buttonMap3G = GameObject.Find("ButtonMap3");
        Button buttonMap3 = _buttonMap3G.GetComponent<Button>();
        buttonMap3.onClick.AddListener(() => { setUpMap(3); });


        //Boutton pour chercher une partie
        _buttonValG = GameObject.Find("ButtonValider");
        Button buttonVal = _buttonValG.GetComponent<Button>();
        buttonVal.onClick.AddListener(() => { StartServer(); });


        //Boutton pour Rechercher une Partie
        _buttonSearchG = GameObject.Find("ButtonSearch");
        Button buttonSearch = _buttonSearchG.GetComponent<Button>();
        buttonSearch.onClick.AddListener(() => { RefreshHostList(); });

        _lobbyName = GameObject.Find("LobbyName");
        _player1name = GameObject.Find("Player1Lobby");
        _player1name.SetActive(false);
        _player2name = GameObject.Find("Player2Lobby");
        _player2name.SetActive(false);
        _player3name = GameObject.Find("Player3Lobby");
        _player3name.SetActive(false);
        _buttonValLobby = GameObject.Find("ButtonValLobby");
        Button buttonValLobby = _buttonValLobby.GetComponent<Button>();
        buttonValLobby.onClick.AddListener(() => { StartGame(); });
        _buttonValLobby.SetActive(false);
        _canvasLobby.SetActive(false);

    }

    /*********************************************************************\
    |   Update : Rafraichis le tableau des parties disponibles			   |
    \*********************************************************************/
    void Update()
    {
        Debug.Log(MasterServer.PollHostList().Length);

        if (_isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            _isRefreshingHostList = false;
            _hostList = MasterServer.PollHostList();
        }

        //Affichage du lancement de partie si les choix du joueur sont terminés
        if (_mapChoose && _playerChoose)
        {
            Vector3 buttonV_position = new Vector3(_buttonValG.transform.position.x, 50.0f, _buttonValG.transform.position.z);
            _buttonValG.transform.position = buttonV_position;
        }
        if (Network.isServer && _player1name.active && _player2name.active && _player3name.active)
        {
            _buttonValLobby.SetActive(true);
            _isReadytoLaunch = true;
        }
        if (_isReadytoInstanciate)
        {
            spawnPlayer();
        }

    }

    /*********************************************************************\
    |   setUpCharacter : crée un joueur de type passé en paramètre	       |
    \*********************************************************************/
    public void setUpCharacter(int i)
    {
        switch (i)
        {
            case 1:
                _playerPrefab = _warriorPrefab;
                break;
            case 2:
                _playerPrefab = _priestPrefab;
                break;
            case 3:
                _playerPrefab = _archerPrefab;
                break;
        }
        _playerChoose = true;
    }

    /*********************************************************************\
    |   setUpMap : crée la carte passée en paramètre	      	     	   |
    \*********************************************************************/
    public void setUpMap(int i)
    {
        switch (i)
        {
            case 1:
                _mapName = "Map no1";
                break;
            case 2:
                _mapName = "Map no2";
                break;
            case 3:
                _mapName = "Map no3";
                break;
        }
        _mapChoose = true;
    }

    /*********************************************************************\
    |   OnGUI Interface de création et de démarage d'une partie			  |
    \*********************************************************************/
    void OnGUI()
    {
        if (_playerChoose && _mapChoose)
        {
            if (MasterServer.PollHostList().Length > 0)
            {
                _hostList = MasterServer.PollHostList();

                Vector3 buttonS_position = new Vector3(_buttonSearchG.transform.position.x, 50.0f, _buttonSearchG.transform.position.z);
                _buttonSearchG.transform.position = buttonS_position;

                for (int i = 0; i < _hostList.Length; i++)
                {   
                    if (GUI.Button(new Rect(600, 100 + (110 * i), 150, 50), _gameName))
                    {
                        JoinServer(_hostList[i]);
                    }
                }
            }
        }
    }


    /*********************************************************************\
    |   StartServer : Initialise un serveur avec 3 joueurs maximum		  |
    \*********************************************************************/
    private void StartServer()
    {
        _canvasPlayers.SetActive(false);

        Network.InitializeServer(3, 25000, !Network.HavePublicAddress());

        //Local MasterServer
        /*
        MasterServer.ipAddress = "127.0.0.1";
        MasterServer.port = 23466;
        Network.natFacilitatorIP = "127.0.0.1";
        Network.natFacilitatorPort = 5005;
        Network.InitializeServer(3, 5005,false);
        */
        MasterServer.RegisterHost(_gameName, "4A Unity Project");
    }

    private void StartGame()
    {
        _isReadytoInstanciate = true;
    }


    /**********************************************************************************\
    |   OnServerInitialized : Création du Gameobject player and le joueur se connecte  |
    \**********************************************************************************/
    void OnServerInitialized()
    {
        spawnPlayer();
        /*
        if (Network.isServer)
        {
            _canvasLobby.SetActive(true);
            _lobbyName.GetComponent<Text>().text = _mapName;
            _player1name.SetActive(true);
            _player1name.GetComponent<Text>().text = GetName(_playerPrefab.name);
        }
        */
    }

    /**********************************************************************************\
    |   GetName : Renvoie le nom du perso choisi                                        |
    \**********************************************************************************/
    private string GetName(string namep)
    {
        string name = null;
        switch (namep)
        {
            case "Player_warrior":
                name = "Warrior";
                break;
            case "Player_archer":
                name = "Archer";
                break;
            case "Player_priest":
                name = "Priest";
                break;
        }
        return name;
    }

    /*********************************************************************\
    |   RefreshHosList : Rafraichis le tableau des parties disponibles	   |
    \*********************************************************************/
    private void RefreshHostList()
    {
        if (!_isRefreshingHostList)
        {
            _isRefreshingHostList = true;
            MasterServer.RequestHostList(_gameName);
        }
    }

    /*********************************************************************\
    |   JoinServer : Connecte le client au serveur						   |
    \*********************************************************************/
    private void JoinServer(HostData hostData)
    {
        /*if (Network.isServer)
        {
            networkView.RPC ("addPlayer", RPCMode.All);
        }*/
        Network.Connect(hostData);
    }

    /*********************************************************************\
    |   addPlayer : Ajoute + 1 au nombres de joueurs connectés			   |
    \*********************************************************************/
    [RPC] //not working
    void addPlayer()
    {
        _numberOfPlayer += 1;
    }

    /*********************************************************************\
    |   OnConnectedToServer : Crée un joueur à la connexion du client	   |
    \*********************************************************************/
    void OnConnectedToServer()
    {
        _canvasPlayers.SetActive(false);
        GUI.enabled = false;
        spawnPlayer();
        /*
        _canvasPlayers.SetActive(false);
        _canvasLobby.SetActive(true);
        if (Network.isClient)
		{
            if (_player1name.active && !_player2name.active && !_player3name.active)
            {
                _player2name.SetActive(true);
                _player2name.GetComponent<Text>().text = GetName(_playerPrefab.name);
            }
            else if (_player2name.active && _player1name.active && !_player3name.active)
            {
                _player3name.SetActive(true);
                _player3name.GetComponent<Text>().text = GetName(_playerPrefab.name);
            }
		}
        */
    }

    /*********************************************************************\
    |   spawnPlayer : Crée le gameObject du joueur a partir d'un prefab	   |
    \*********************************************************************/
    private void spawnPlayer()
    {
        Network.Instantiate(_playerPrefab, Vector3.up * 5, Quaternion.identity, 0);

    }

}