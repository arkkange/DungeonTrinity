using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private const string _typeName = "Dungeon Trinity"; 
    private const string _gameName = "Maps no1"; 

    private bool _isRefreshingHostList = false;
    private HostData[] _hostList; 
	private int _numberOfPlayer = 0;

    public GameObject _playerPrefab;

	
	/*********************************************************************\
    |   OnGUI Interface de création et de démarage d'une partie			  |
    \*********************************************************************/
    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
			if (_hostList == null)
			{
            	if (GUI.Button(new Rect(100, 100, 250, 100), "Demarrer le Serveur"))  
                	StartServer();
			}

			if (_hostList != null)
			{
            if (GUI.Button(new Rect(100, 100, 250, 100), "Rechercher une Partie"))
                RefreshHostList();

                for (int i = 0; i < _hostList.Length; i++)
                {
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), _hostList[i].gameName ))
                        JoinServer(_hostList[i]);
                }
            }
        }
    }

	
	/*********************************************************************\
    |   StartServer : Initialise un serveur avec 3 joueurs maximum		  |
    \*********************************************************************/
    private void StartServer()
    {
        Network.InitializeServer(3, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(_typeName, _gameName);
    }

	
	/**********************************************************************************\
    |   OnServerInitialized : Création du Gameobject player and le joueur se connecte  |
    \**********************************************************************************/
    void OnServerInitialized()
    {
		if (Network.isClient)
		{		
        	spawnPlayer();
		}
	}

	void Start()
	{
		RefreshHostList();
	}

	/*********************************************************************\
    |   Update : Rafraichis le tableau des parties disponibles			   |
    \*********************************************************************/
    void Update()
    {
        if (_isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            _isRefreshingHostList = false;
            _hostList = MasterServer.PollHostList();
        }
    }

	
	/*********************************************************************\
    |   RefreshHosList : Rafraichis le tableau des parties disponibles	   |
    \*********************************************************************/
    private void RefreshHostList()
    {
        if (!_isRefreshingHostList)
        {
            _isRefreshingHostList = true;
            MasterServer.RequestHostList(_typeName);
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
		if (Network.isClient)
		{		
			spawnPlayer();			
		}
	}

	/*********************************************************************\
    |   spawnPlayer : Crée le gameObject du joueur a partir d'un prefab	   |
    \*********************************************************************/
    private void spawnPlayer()
    {
    	Network.Instantiate(_playerPrefab, Vector3.up * 5, Quaternion.identity, 0);
    
	}
	
}