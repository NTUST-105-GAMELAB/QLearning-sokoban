using System.Collections.Generic;
using UnityEngine;

//-------------for AI.---------
using Enum = System.Enum;
using System.Linq;

public class Main : MonoBehaviour {

	public Spot spotPrefab;
	public GameObject spotParrent;

	// The map consists of many spot.
	public List<List<Spot>> map;
	private int mapSize;
	// The position of box1.
	private int box1_x, box1_z;
	// The next position of box1.
	private int box1_next_x, box1_next_z;
	// The position of player.
	private int player_x, player_z;
	// The next position of player.
	private int player_next_x, player_next_z;
	// The position of point1.
	private int point1_x, point1_z;
	// The position of point2.
	private int point2_x, point2_z;

	// Use this for initialization
	void Start() {
		// Initial
		mapSize = 10;
		//boxPos_1 = new Vector3(2, 2, 2);
		box1_x = 2;
		box1_z = 2;
		box1_next_x = box1_x;
		box1_next_z = box1_z;
		player_x = 1;
		player_z = 1;
		player_next_x = player_x;
		player_next_z = player_z;
		point1_x = 8;
		point1_z = 8;
		//point2_x = 5;
		//point2_z = 6;

		// Create new map.
		map = new List<List<Spot>>();
		for (int x = 0; x < mapSize; x++) {
			map.Add(new List<Spot>());
			for (int z = 0; z < mapSize; z++) {
				// Clone the spots.
				Spot spotChild = GameObject.Instantiate<Spot>(spotPrefab);
				spotChild.transform.SetParent(spotParrent.transform);
				spotChild.transform.position = new Vector3(x, 0, z);
				map[x].Add(spotChild);
			}
		}
		// Set the spot be the wall.
		for (int i = 0; i < mapSize; i++) {
			map[i][0].spotType = Spot.SPOT_TYPE.WALL;
			map[i][mapSize - 1].spotType = Spot.SPOT_TYPE.WALL;
		}
		for (int j = 1; j < mapSize - 1; j++) {
			map[0][j].spotType = Spot.SPOT_TYPE.WALL;
			map[mapSize - 1][j].spotType = Spot.SPOT_TYPE.WALL;
		}
		// Set the spot be the start point.
		map[player_x][player_z].spotType = Spot.SPOT_TYPE.START;

		// Set the spot be the box point.
		map[point1_x][point1_z].spotType = Spot.SPOT_TYPE.POINT;
		//map[point2_x][point2_z].spotType = Spot.SPOT_TYPE.POINT;

		// Set the spot be the box initial point.
		map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXOFF;

		//for ai to init reward map.
		initRewardMap();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			AI(10000, 20);
		}

		if (Input.GetKeyDown(KeyCode.A)) {
			show();
		}

		if (Input.GetKeyDown(KeyCode.Z)) {
			ReStart();
		}

		map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXOFF;
		map[point1_x][point1_z].spotType = Spot.SPOT_TYPE.POINT;
		map[player_x][player_z].spotType = Spot.SPOT_TYPE.START;

		// When the player move... 
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			showQ(Action.Up);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			showQ(Action.Down);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			showQ(Action.Rigth);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			showQ(Action.Left);
		}

		//// Player can move to there or not.
		//if (map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.WALL &&
		//	map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.BOXOFF &&
		//	map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.BOXON) {
		//	map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
		//	player_x = player_next_x;
		//	player_z = player_next_z;

		//	// Move the box.
		//}
		//else if (map[player_next_x][player_next_z].spotType == Spot.SPOT_TYPE.BOXOFF ||
		//		  map[player_next_x][player_next_z].spotType == Spot.SPOT_TYPE.BOXON) {
		//	// box
		//	box1_next_x = box1_x + (player_next_x - player_x);
		//	box1_next_z = box1_z + (player_next_z - player_z);

		//	if (map[box1_next_x][box1_next_z].spotType != Spot.SPOT_TYPE.WALL &&
		//		map[box1_next_x][box1_next_z].spotType != Spot.SPOT_TYPE.POINT) {
		//		box1_x = box1_next_x;
		//		box1_z = box1_next_z;
		//		map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXOFF;

		//		// player
		//		map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
		//		player_x = player_next_x;
		//		player_z = player_next_z;

		//	}
		//	else if (map[box1_next_x][box1_next_z].spotType == Spot.SPOT_TYPE.POINT) {
		//		box1_x = box1_next_x;
		//		box1_z = box1_next_z;
		//		map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXON;

		//		// player
		//		map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
		//		player_x = player_next_x;
		//		player_z = player_next_z;

		//	}
		//	else {
		//		box1_next_x = box1_x;
		//		box1_next_z = box1_z;
		//	}
		//}

		//// Show the point.
		//if (map[point1_x][point1_z].spotType == Spot.SPOT_TYPE.FLOOR) {
		//	map[point1_x][point1_z].spotType = Spot.SPOT_TYPE.POINT;

		//}
		////else if (map[point2_x][point2_z].spotType == Spot.SPOT_TYPE.FLOOR) {
		////	map[point2_x][point2_z].spotType = Spot.SPOT_TYPE.POINT;
		////}

		//player_next_x = player_x;
		//player_next_z = player_z;
		//map[player_x][player_z].spotType = Spot.SPOT_TYPE.START;
	}

	#region forAI.

	public class QValue {
		//the up ,down,left,right reward.
		float[] rewards;
		public int position_x {
			get;
			private set;
		}
		public int position_z {
			get;
			private set;
		}

		public QValue(int position_x, int position_z) {
			rewards = new float[4];
			this.position_x = position_x;
			this.position_z = position_z;
		}

		public Action bestDirect() {
			if (rewards.All(x => x.Equals(rewards.First())))
				return (Action)Random.Range(0, Enum.GetNames(typeof(Action)).Length);
			return (Action)rewards.ToList().IndexOf(rewards.Max());
		}

		public float bestReward() {
			return rewards.Max();
		}

		//API for change reward. 
		public void modifyReward(Action action, float value) {
			rewards[(int)action] = value;
		}

		//if this point is wall, then let it's rewards array to float_min.
		public void isWall() {
			rewards = Enumerable.Repeat(float.MinValue, 4).ToArray();
		}

		//for show the QValueMap.
		public float showQValue(Action action) {
			return rewards[(int)action];
		}
	}

	QValue[,] RewardMap = new QValue[10, 10];
	readonly float LearningRate = 0.4f;
	readonly float disRate = 0.8f;

	public enum Action {
		Up,
		Down,
		Left,
		Rigth
	}

	public void AI(int maxRound, int maxTime) {
		//run maxRAnge times.
		for (var round = 0; round < maxRound; ++round) {
			//one round do the max time.
			for (var time = 0; time < maxTime; ++time) {
				Move(RewardMap[player_x, player_z].bestDirect());
				//Move((Action)Random.Range(0, Enum.GetNames(typeof(Action)).Length));
				//if push the box to the goal, then end this round.
				if (box1_x == point1_x && box1_z == point1_z)
					break;
			}
			ReStart();
		}
		Debug.Log("done");
	}

	public float Reward() {
		int deltaPlayer_x = player_next_x - player_x;
		int deltaPlayer_z = player_next_z - player_z;
		if (player_next_x == box1_x && player_next_z == box1_z) {
			//box pushed on goal.
			if ((box1_x + deltaPlayer_x) == point1_x && (box1_z + deltaPlayer_z) == point1_z) {
				return 1.0f;
			}
			//push the box.
			return -0.1f;
		}
		//go to the wall.
		if(map[player_next_x][player_next_z].spotType ==Spot.SPOT_TYPE.WALL) {
			return -0.2f;
		}
		//don't push box.
		return -0.15f;
	}

	//move player for ai.
	public void Move(Action action) {
		switch (action) {
			case Action.Up:
				player_next_z = player_z + 1;
				BellmanFunction(Action.Up);
				if (CanWalk(player_next_x, player_next_z)) {
					//bellmanfunction();
					MoveBox();
				}
				break;
			case Action.Down:
				player_next_z = player_z - 1;
				BellmanFunction(Action.Down);
				if (CanWalk(player_next_x, player_next_z)) {
					//bellmanfunction();
					MoveBox();
				}
				break;
			case Action.Rigth:
				player_next_x = player_x + 1;
				BellmanFunction(Action.Rigth);
				if (CanWalk(player_next_x, player_next_z)) {
					//bellmanfunction();
					MoveBox();
				}
				break;
			case Action.Left:
				player_next_x = player_x - 1;
				BellmanFunction(Action.Left);
				if (CanWalk(player_next_x, player_next_z)) {
					//bellmanfunction();
					MoveBox();
				}
				break;
		}
	}

	//can walk or not.
	public bool CanWalk(int wantToX, int wantToY) {
		// Player can move to there or not.
		if (map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.WALL) {
			return true;
		}
		return false;
	}

	//move box or not.
	public void MoveBox() {
		if (map[player_next_x][player_next_z].spotType == Spot.SPOT_TYPE.BOXOFF ||
			  map[player_next_x][player_next_z].spotType == Spot.SPOT_TYPE.BOXON) {
			// box
			box1_next_x = box1_x + (player_next_x - player_x);
			box1_next_z = box1_z + (player_next_z - player_z);

			if (map[box1_next_x][box1_next_z].spotType != Spot.SPOT_TYPE.WALL &&
				map[box1_next_x][box1_next_z].spotType != Spot.SPOT_TYPE.POINT) {
				box1_x = box1_next_x;
				box1_z = box1_next_z;
				map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXOFF;

				// player
				map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
				player_x = player_next_x;
				player_z = player_next_z;

			}
			else if (map[box1_next_x][box1_next_z].spotType == Spot.SPOT_TYPE.POINT) {
				box1_x = box1_next_x;
				box1_z = box1_next_z;
				map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXON;

				// player
				map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
				player_x = player_next_x;
				player_z = player_next_z;

			}
			else {
				box1_next_x = box1_x;
				box1_next_z = box1_z;
			}
		}

		map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
		player_x = player_next_x;
		player_z = player_next_z;

		player_next_x = player_x;
		player_next_z = player_z;
		map[player_x][player_z].spotType = Spot.SPOT_TYPE.START;
	}

	//calculate bellman function.
	public void BellmanFunction(Action action) {
		float temp = (1.0f - LearningRate) * RewardMap[player_x, player_z].bestReward() + LearningRate * (Reward() + disRate * RewardMap[player_next_x, player_next_z].bestReward());
		RewardMap[player_x, player_z].modifyReward(action, temp);
	}

	public void ReStart() {
		// Set the spot be the box initial point.
		map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.FLOOR;
		// Set the spot be the start point.
		map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;

		box1_x = 2;
		box1_z = 2;
		box1_next_x = box1_x;
		box1_next_z = box1_z;
		player_x = 1;
		player_z = 1;
		player_next_x = player_x;
		player_next_z = player_z;
		// Set the spot be the start point.
		map[player_x][player_z].spotType = Spot.SPOT_TYPE.START;
		// Set the spot be the box point.
		map[point1_x][point1_z].spotType = Spot.SPOT_TYPE.POINT;
		// Set the spot be the box initial point.
		map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXOFF;
	}

	public void initRewardMap() {
		for (int i = 0; i < mapSize; ++i) {
			for (int j = 0; j < mapSize; ++j) {
				RewardMap[i, j] = new QValue(i, j);
				if (map[i][j].spotType == Spot.SPOT_TYPE.WALL)
					RewardMap[i, j].isWall();
			}
		}
		//for (int i = 1; i < mapSize - 1; ++i) {
		//	RewardMap[i, 8].modifyReward(Action.Up, float.MinValue);
		//	RewardMap[i, 1].modifyReward(Action.Down, float.MinValue);
		//	RewardMap[1, i].modifyReward(Action.Left, float.MinValue);
		//	RewardMap[8, i].modifyReward(Action.Rigth, float.MinValue);
		//}
	}

	public void showMove(Action action) {
		switch (action) {
			case Action.Up:
				player_next_z = player_z + 1;
				test();
				//if (CanWalk(player_next_x, player_next_z)) {
				//	MoveBox();
				//}
				break;
			case Action.Down:
				player_next_z = player_z - 1;
				test();
				//if (CanWalk(player_next_x, player_next_z)) {
				//	MoveBox();
				//}
				break;
			case Action.Rigth:
				player_next_x = player_x + 1;
				test();
				//if (CanWalk(player_next_x, player_next_z)) {
				//	MoveBox();
				//}
				break;
			case Action.Left:
				player_next_x = player_x - 1;
				test();
				//if (CanWalk(player_next_x, player_next_z)) {
				//	MoveBox();
				//}
				break;
		}
	}

	public void show() {
		Debug.Log("test");
		int i = 0;
		while (i < 20) {
			Debug.Log(RewardMap[player_x, player_z].bestDirect());
			showMove(RewardMap[player_x, player_z].bestDirect());
			if (box1_x == point1_x && box1_z == point1_z)
				break;
			i++;
		}
		Debug.Log(player_x + " " + player_z);
		Debug.Log(box1_x + " " + box1_z);
	}

	public void showQ(Action action) {
		string QValueMap = default(string);
		for (int j = 8; j > 0; --j) {
			for (int i = 1; i < mapSize - 1; ++i) {
				QValueMap += RewardMap[i, j].showQValue(action).ToString() + " ";
			}
			QValueMap += "\n";
		}
		Debug.Log(QValueMap);
	}

	public void test() {
		// Player can move to there or not.
		if (map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.WALL &&
			map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.BOXOFF &&
			map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.BOXON) {
			map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
			player_x = player_next_x;
			player_z = player_next_z;

			// Move the box.
		}
		else if (map[player_next_x][player_next_z].spotType == Spot.SPOT_TYPE.BOXOFF ||
				  map[player_next_x][player_next_z].spotType == Spot.SPOT_TYPE.BOXON) {
			// box
			box1_next_x = box1_x + (player_next_x - player_x);
			box1_next_z = box1_z + (player_next_z - player_z);

			if (map[box1_next_x][box1_next_z].spotType != Spot.SPOT_TYPE.WALL &&
				map[box1_next_x][box1_next_z].spotType != Spot.SPOT_TYPE.POINT) {
				box1_x = box1_next_x;
				box1_z = box1_next_z;
				map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXOFF;

				// player
				map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
				player_x = player_next_x;
				player_z = player_next_z;

			}
			else if (map[box1_next_x][box1_next_z].spotType == Spot.SPOT_TYPE.POINT) {
				box1_x = box1_next_x;
				box1_z = box1_next_z;
				map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXON;

				// player
				map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
				player_x = player_next_x;
				player_z = player_next_z;

			}
			else {
				box1_next_x = box1_x;
				box1_next_z = box1_z;
			}
		}

		// Show the point.
		if (map[point1_x][point1_z].spotType == Spot.SPOT_TYPE.FLOOR) {
			map[point1_x][point1_z].spotType = Spot.SPOT_TYPE.POINT;

		}
		//else if (map[point2_x][point2_z].spotType == Spot.SPOT_TYPE.FLOOR) {
		//	map[point2_x][point2_z].spotType = Spot.SPOT_TYPE.POINT;
		//}

		player_next_x = player_x;
		player_next_z = player_z;
		map[player_x][player_z].spotType = Spot.SPOT_TYPE.START;
	}

	#endregion
}
