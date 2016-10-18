﻿using UnityEngine;
using System.Collections;


public class BowlingManager : MonoBehaviour {

    //event declaration for spawning pins
    public delegate void SpawnAction(GameObject obj);
    public static event SpawnAction SpawnPin;

    //event declaration for cleaning up pins
    public delegate void CleanAction();
    public static event CleanAction DestroyPin;



    public Transform ballSpawner;
    public GameObject pinObject;
    public GameObject ballObject;

    //final score of the player so far
    int finalScore = 0;

    //the amount the player earns during each frame (2 throws)
    //12 frames for 10 frames + 2 bonus rounds
    int[] frameScore = new int[12] {0,0,0,0,0,0,0,0,0,0,0,0};

    //if a particular frame was a strike
    bool[] frameStrike = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };



    //checks if this frame is on its second round
    bool round2 = false;

    //current frame (round)
    int currentFrame = 0;
    const int MAX_FRAMES = 10;

    //subscribe BallDrop function to LostBall Event
    //When a ball hits a gutter, calls BallDrop() to bring it back
    void OnEnable()
    {Gutter.LostBall += BallDrop; Gutter.HitPin += IncreaseScore;}
    void OnDisable()
    {Gutter.LostBall -= BallDrop; Gutter.HitPin += IncreaseScore;}

    // Use this for initialization
    void Start()
    {
        //start first frame
        StartFrame();
    }



    //runs at the beginning of each frame, resets pins and points counter for frame
    void StartFrame()
    {
        //indicate that this frame is on its first round
        round2 = false;


        //if all 10 frames have been finished
        if(currentFrame > MAX_FRAMES)
        {
            //end the game
            EndGame();
        }
        else
        {
            Debug.Log("Frame "+ currentFrame+" begin!");
            //reset frame score
            frameScore[currentFrame] = 0;

            //spawn the ball
            RespawnBall();
            //spawn the pins
            RespawnPins();
            //output current scores
            OutputScore();
        }
    }
    //start second round of current frame
    void Round2()
    {
        //indicate that this frame has started its second round
        round2 = true;

        Debug.Log("Frame " + currentFrame + " round 2!");
       
        //spawn the ball
        RespawnBall();
       
        //output current scores
        OutputScore();
    }

    void EndFrame()
    {
        //calculate final score so far
        ScoreHandler();

        //clean up remaining pins by calling DestroyPin Event
        DestroyPin();

        //increase frame count
        currentFrame++;

        //start next frame
        StartFrame();
    }

    //handles proper behavior when a ball is destroyed
    //Gutterball or ball lands in back pit
    void BallDrop()
    {
        //if this was the first round
        if (!round2)
        {
            //check if a strike was made
            if (frameScore[currentFrame] == 10)
            {
                Debug.Log("STRIKE!!!!!!!!!!!!!!!");
                //indicate that a strike occured on this frame
                frameStrike[currentFrame] = true;
                //End this frame
                EndFrame();
            }
            else
            {
                Debug.Log("You hit " + frameScore + " pins!");
                //start round 2
                Round2();
            }

        }
        else //if this was the second round
        {
            //check if a spare was made
            if (frameScore[currentFrame] == 10)
            {
                Debug.Log("SPARE!");
            }
            else
            {
                Debug.Log("You hit " + frameScore + " pins!");
            }
        }
    }

    void ScoreHandler()
    {
        //CALCULATE FINAL SCORING
        //check for strike last frame (if this isnt the first frame)
        if (currentFrame > 0 && frameStrike[currentFrame-1] == true)
        {
            frameScore[currentFrame - 1] += frameScore[currentFrame];     
        }

        //check for strike 2 frames ago (if this isnt the first or second frame)
        if (currentFrame > 1 && frameStrike[currentFrame - 2] == true)
        {
            frameScore[currentFrame - 2] += frameScore[currentFrame];    
        }

        //if this was a strike
        if(frameStrike[currentFrame] == true)
        {
            //start at 1 since a strike just occurred
            int strikeCounter = 1;
            //count back from the current frame to see how many strikes occured in sequence
            for(int i = currentFrame-1; i >= 0; i-- )
            {
                //if the frame being checked was a strike
                if(frameStrike[i] == true)
                {
                    strikeCounter++;
                }
                else
                {
                    break;
                }
            }

            switch(strikeCounter)
            {
                case 3:
                    Debug.Log("Turkey!!!");
                    break;
                case 6:
                    Debug.Log("Wild Turkey!!!");
                    break;
                case 9:
                    Debug.Log("Gold Turkey!!!");
                    break;
                case 12:
                    Debug.Log("PERFECT GAME!!!");
                    break;
                default:
                    Debug.Log("strike counter is at: "+strikeCounter);
                    break;
            }

           
        }

        //calculate current final score
        //add all scores together
        for (int i = 0; i <= currentFrame; i++)
        {
            finalScore += frameScore[i];
        }
    }

    //increases the player score
    void IncreaseScore()
    {
        //increase the frame score by one
        frameScore[currentFrame]++;
    }

   
    void RespawnBall()
    {
        //if the ball object has been assigned
        if(ballObject != null)
        {
            //instantiate pin
            GameObject ball = Instantiate(ballObject, ballSpawner.position, ballSpawner.rotation) as GameObject;
        }
       
    }

    void RespawnPins()
    {
        if (SpawnPin != null)
        {
            SpawnPin(pinObject);
        }  
    }
    void OutputScore()
    {
        Debug.Log("Frame Score: "+ frameScore);
    }
    void EndGame()
    {
        //calculate score

        //display score

        //save score?

        //retry or exit
    }
}
