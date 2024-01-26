using BackEnd;
using BackEnd.Tcp;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchSystem
{

    public MatchCard GetMatchList(int index)
    {
        var callback = Backend.Match.GetMatchList();

        if (!callback.IsSuccess())
        {
            Debug.LogError("Backend.Match.GetMatchList Error : " + callback);
            return null;
        }

        List<MatchCard> matchCardList = new List<MatchCard>();

        LitJson.JsonData matchCardListJson = callback.FlattenRows();



        for (int i = 0; i < matchCardListJson.Count; i++)
        {
            MatchCard matchCard = new MatchCard();

            matchCard.inDate = matchCardListJson[i]["inDate"].ToString();
            matchCard.result_processing_type = matchCardListJson[i]["result_processing_type"].ToString();
            matchCard.version = int.Parse(matchCardListJson[i]["version"].ToString());
            matchCard.matchTitle = matchCardListJson[i]["matchTitle"].ToString();
            matchCard.enable_sandbox = matchCardListJson[i]["enable_sandbox"].ToString() == "true" ? true : false;
            matchCard.matchType = matchCardListJson[i]["matchType"].ToString();
            matchCard.matchModeType = matchCardListJson[i]["matchModeType"].ToString();

            foreach (LitJson.JsonData row in callback.Rows())
            {

                foreach (MatchType type in Enum.GetValues(typeof(MatchType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchType"]["S"].ToString().ToLower()))
                    {
                        matchCard.matchTypeEnum = type;
                    }
                }

                foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchModeType"]["S"].ToString().ToLower()))
                    {
                        matchCard.matchModeTypeEnum = type;
                    }
                }
            }

            matchCard.matchHeadCount = int.Parse(matchCardListJson[i]["matchHeadCount"].ToString());
            matchCard.enable_battle_royale = matchCardListJson[i]["enable_battle_royale"].ToString() == "true" ? true : false;
            matchCard.match_timeout_m = int.Parse(matchCardListJson[i]["match_timeout_m"].ToString());
            matchCard.transit_to_sandbox_timeout_ms = int.Parse(matchCardListJson[i]["transit_to_sandbox_timeout_ms"].ToString());
            matchCard.match_start_waiting_time_s = int.Parse(matchCardListJson[i]["match_start_waiting_time_s"].ToString());

            if (matchCardListJson[i].ContainsKey("match_increment_time_s"))
            {
                matchCard.match_increment_time_s = int.Parse(matchCardListJson[i]["match_increment_time_s"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("maxMatchRange"))
            {
                matchCard.maxMatchRange = int.Parse(matchCardListJson[i]["maxMatchRange"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("increaseAndDecrease"))
            {
                matchCard.increaseAndDecrease = int.Parse(matchCardListJson[i]["increaseAndDecrease"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("initializeCycle"))
            {
                matchCard.initializeCycle = matchCardListJson[i]["initializeCycle"].ToString();
            }
            if (matchCardListJson[i].ContainsKey("defaultPoint"))
            {
                matchCard.defaultPoint = int.Parse(matchCardListJson[i]["defaultPoint"].ToString());
            }

            if (matchCardListJson[i].ContainsKey("savingPoint"))
            {
                if (matchCardListJson[i]["savingPoint"].IsArray)
                {
                    for (int listNum = 0; listNum < matchCardListJson[i]["savingPoint"].Count; listNum++)
                    {
                        var keyList = matchCardListJson[i]["savingPoint"][listNum].Keys;
                        foreach (var key in keyList)
                        {
                            matchCard.savingPoint.Add(key, int.Parse(matchCardListJson[i]["savingPoint"][listNum][key].ToString()));
                        }
                    }
                }
                else
                {
                    foreach (var key in matchCardListJson[i]["savingPoint"].Keys)
                    {
                        matchCard.savingPoint.Add(key, int.Parse(matchCardListJson[i]["savingPoint"][key].ToString()));
                    }
                }
            }
            matchCardList.Add(matchCard);
        }

        return matchCardList[index];
    }


    public void GetMatchList()
    {
        var callback = Backend.Match.GetMatchList();

        if (!callback.IsSuccess())
        {
            Debug.LogError("Backend.Match.GetMatchList Error : " + callback);
            return;
        }

        List<MatchCard> matchCardList = new List<MatchCard>();

        LitJson.JsonData matchCardListJson = callback.FlattenRows();

        Debug.Log("Backend.Match.GetMatchList : " + callback);

        for (int i = 0; i < matchCardListJson.Count; i++)
        {
            MatchCard matchCard = new MatchCard();

            matchCard.inDate = matchCardListJson[i]["inDate"].ToString();
            matchCard.result_processing_type = matchCardListJson[i]["result_processing_type"].ToString();
            matchCard.version = int.Parse(matchCardListJson[i]["version"].ToString());
            matchCard.matchTitle = matchCardListJson[i]["matchTitle"].ToString();
            matchCard.enable_sandbox = matchCardListJson[i]["enable_sandbox"].ToString() == "true" ? true : false;


            matchCard.matchType = matchCardListJson[i]["matchType"].ToString();
            matchCard.matchModeType = matchCardListJson[i]["matchModeType"].ToString();

            foreach (LitJson.JsonData row in callback.Rows())
            {

                foreach (MatchType type in Enum.GetValues(typeof(MatchType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchType"]["S"].ToString().ToLower()))
                    {
                        matchCard.matchTypeEnum = type;
                    }
                }

                foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchModeType"]["S"].ToString().ToLower()))
                    {
                        matchCard.matchModeTypeEnum = type;
                    }
                }
            }

            matchCard.matchHeadCount = int.Parse(matchCardListJson[i]["matchHeadCount"].ToString());
            matchCard.enable_battle_royale = matchCardListJson[i]["enable_battle_royale"].ToString() == "true" ? true : false;
            matchCard.match_timeout_m = int.Parse(matchCardListJson[i]["match_timeout_m"].ToString());
            matchCard.transit_to_sandbox_timeout_ms = int.Parse(matchCardListJson[i]["transit_to_sandbox_timeout_ms"].ToString());
            matchCard.match_start_waiting_time_s = int.Parse(matchCardListJson[i]["match_start_waiting_time_s"].ToString());

            if (matchCardListJson[i].ContainsKey("match_increment_time_s"))
            {
                matchCard.match_increment_time_s = int.Parse(matchCardListJson[i]["match_increment_time_s"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("maxMatchRange"))
            {
                matchCard.maxMatchRange = int.Parse(matchCardListJson[i]["maxMatchRange"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("increaseAndDecrease"))
            {
                matchCard.increaseAndDecrease = int.Parse(matchCardListJson[i]["increaseAndDecrease"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("initializeCycle"))
            {
                matchCard.initializeCycle = matchCardListJson[i]["initializeCycle"].ToString();
            }
            if (matchCardListJson[i].ContainsKey("defaultPoint"))
            {
                matchCard.defaultPoint = int.Parse(matchCardListJson[i]["defaultPoint"].ToString());
            }

            if (matchCardListJson[i].ContainsKey("savingPoint"))
            {
                if (matchCardListJson[i]["savingPoint"].IsArray)
                {
                    for (int listNum = 0; listNum < matchCardListJson[i]["savingPoint"].Count; listNum++)
                    {
                        var keyList = matchCardListJson[i]["savingPoint"][listNum].Keys;
                        foreach (var key in keyList)
                        {
                            matchCard.savingPoint.Add(key, int.Parse(matchCardListJson[i]["savingPoint"][listNum][key].ToString()));
                        }
                    }
                }
                else
                {
                    foreach (var key in matchCardListJson[i]["savingPoint"].Keys)
                    {
                        matchCard.savingPoint.Add(key, int.Parse(matchCardListJson[i]["savingPoint"][key].ToString()));
                    }
                }
            }
            matchCardList.Add(matchCard);
        }

        foreach (var matchCard in matchCardList)
        {
            Debug.Log(matchCard.ToString());
        }
    }


    public void JoinMatchMakingCheck()
    {
        ErrorInfo errorInfo;
        Backend.Match.JoinMatchMakingServer(out errorInfo);
        Debug.Log(errorInfo);
    }

    public void JoinMatchMaking()
    {
        ErrorInfo errorInfo;


        Backend.Match.JoinMatchMakingServer(out errorInfo);
        Backend.Match.OnJoinMatchMakingServer = (JoinChannelEventArgs args) =>
        {
            
            Debug.Log(args.ErrInfo);

            CreateMatchRoom();
            RequestMatchMaking(0);
        };
    }

    private void CreateMatchRoom()
    {
        Backend.Match.CreateMatchRoom();
        Backend.Match.OnMatchMakingRoomCreate = (MatchMakingInteractionEventArgs args) =>
        {
            Debug.Log(args.ErrInfo);
        };
    }

    public void RequestMatchMaking(int index)
    {
        Backend.Match.RequestMatchMaking(GetMatchList(index).matchTypeEnum, GetMatchList(index).matchModeTypeEnum, GetMatchList(index).inDate);
        Backend.Match.OnMatchMakingResponse = (MatchMakingResponseEventArgs args) =>
        {
            Debug.Log(args.ErrInfo);
            if(args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log(args.RoomInfo.m_enableSandbox);
            }
        };
    }
}
