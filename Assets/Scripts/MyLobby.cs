using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MyLobby : NetworkLobbyManager {

	IEnumerator Start () {
		yield return 0; //0 will wait 1 frame.

        MMStart();
        MMListMatches();
	}
	
	void MMStart() {
        print("@ Start");

        StartMatchMaker();
    }

    void MMListMatches() {
        print("@ ListMatches");

        matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);

    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
        print("@ OnMatchList");
        base.OnMatchList(success, extendedInfo, matchList);
        
        if (!success) {
            print("List Failed: " + extendedInfo);
        }
        else {
            if (matchList.Count > 0) {
                print("Successfully listed matches. 1st match: " + matchList[0]);
                MMJoinMatch(matchList[0]);
            }
            else {
                MMCreateMatch();
            }
        }
    }

    void MMJoinMatch(MatchInfoSnapshot firstMatch) {
        print("@ JoinMatch");
        //matchMaker.JoinMatch(firstMatch.networkId, "", "", "", 0, 0, OnMatchJoined);
        print("temporary disabled join match!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        matchMaker.CreateMatch("MM", 15, true, "", "", "", 0, 0, OnMatchCreate);
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo) {
        print("@ JoinedMatch");

        base.OnMatchJoined(success, extendedInfo, matchInfo);
        if (!success) {
            print("Failed to join match: " + extendedInfo);
        } else {
            //Success
            print("Successfully joined match: " + matchInfo.networkId);
        }
    }

    void MMCreateMatch() {
        print("@ CreateMatch");
        matchMaker.CreateMatch("MM", 15, true, "", "", "", 0, 0, OnMatchCreate);
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
        print("@ CreatedMatch");

        base.OnMatchCreate(success, extendedInfo, matchInfo);
        if (!success) {
            print("Failed to join match: " + extendedInfo);
        } else {
            //sucess
            print("Successfully joined match: " + matchInfo.networkId);
        }
    }
}
