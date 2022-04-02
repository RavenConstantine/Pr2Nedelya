using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    Dictionary <string, float> bids  = new Dictionary <string, float>();
    float balans;
    int bidCell;
    float bank;
    public int winCell;
    public string winColor;
    public Text bidText;
    public Text bankText;
    public Text balansText;
    public float grRoul=0, grBall=0, grRoulNow, grBallNow;
    public float rotationSpeed = 100; 
    public GameObject decor;
    float LT=0;
    int [] winCellRatio = {0, 23, 6, 35, 4, 19, 10, 31, 16, 27, 18, 14, 33, 12, 25, 2, 21, 8, 29, 3, 24, 5, 28, 17, 20, 7, 36, 11, 32, 30, 15, 26, 1, 22, 9, 34, 13};
    void Start() {
        bank=2000;
        balans=1000;
        UpdateTexts();
    }
    void GetWinInf(){
        winCell = Random.Range(0,37);
        int ostCol = winCell%2;
        if (winCell == 0)
            winColor = "Green";
        else if (winCell<=10){
            if(ostCol==1)
                winColor = "Red";
            else
                winColor = "Black";
        }
        else if (winCell<=18){
            if(ostCol==0)
                winColor = "Red";
            else
                winColor = "Black";
        }
        else if (winCell<=28){
            if(ostCol==1)
                winColor = "Red";
            else
                winColor = "Black";
        }
        else{
            if(ostCol==0)
                winColor = "Red";
            else
                winColor = "Black";
        }
    }
    void CheckPlayerBids(){
        if(bids.ContainsKey(winCell.ToString())){
            balans+=bids[winCell.ToString()]*36;
            bank-=bids[winCell.ToString()]*36;
        }
        if(winCell!=0){
            if(bids.ContainsKey(winColor)){
                balans+=bids[winColor]*2;
                bank-=bids[winColor]*2;
            }
            if(bids.ContainsKey("Even")){
                if(winCell%2==0){
                    balans+=bids["Even"]*2;
                    bank-=bids["Even"]*2;
                }
            }
            if(bids.ContainsKey("Odd")){
                if(winCell%2==1){
                    balans+=bids["Odd"]*2;
                    bank-=bids["Odd"]*2;
                }
            }
            if(bids.ContainsKey("1-18")){
                if(winCell<=18&&winCell>=1){
                    balans+=bids["1-18"]*2;
                    bank-=bids["1-18"]*2;
                }
            }
            if(bids.ContainsKey("19-36")){
                if(winCell<=36&&winCell>=19){
                    balans+=bids["19-36"]*2;
                    bank-=bids["19-36"]*2;
                }
            }
            if(bids.ContainsKey("1-12")){
                if(winCell<=12&&winCell>=1){
                    balans+=bids["1-12"]*3;
                    bank-=bids["1-12"]*3;
                }
            }
            if(bids.ContainsKey("13-24")){
                if(winCell<=24&&winCell>=13){
                    balans+=bids["13-24"]*3;
                    bank-=bids["13-24"]*3;
                }
            }
            if(bids.ContainsKey("25-36")){
                if(winCell<=36&&winCell>=25){
                    balans+=bids["25-36"]*3;
                    bank-=bids["25-36"]*3;
                }
            }
            if(bids.ContainsKey("1st")){
                if(winCell%3==1){
                    balans+=bids["1st"]*3;
                    bank-=bids["1st"]*3;
                }
            }
            if(bids.ContainsKey("2nd")){
                if(winCell%3==2){
                    balans+=bids["2nd"]*3;
                    bank-=bids["2nd"]*3;
                }
            }
            if(bids.ContainsKey("3rd")){
                if(winCell%3==0){
                    balans+=bids["3rd"]*3;
                    bank-=bids["3rd"]*3;
                }
            }
        }
    }
    public void AddBid(string str){
        if(float.TryParse(bidText.text, out float bid)){
            if(bid>0&&bid<=balans){
                balans-=bid;
                bank+=bid;
                if(bids.ContainsKey(str))
                    bids[str]+=bid;
                else
                    bids.Add(str, bid);
            }
            GameObject inst = Instantiate(Resources.Load ("Chip")) as GameObject;
            inst.transform.parent = GameObject.Find(str).transform;
            inst.GetComponent<RectTransform>().localScale = new Vector3(inst.GetComponent<RectTransform>().localScale.x * GameObject.Find("ControlScale").GetComponent<RectTransform>().lossyScale.x,inst.GetComponent<RectTransform>().localScale.y * GameObject.Find("ControlScale").GetComponent<RectTransform>().lossyScale.y,inst.GetComponent<RectTransform>().localScale.z * GameObject.Find("ControlScale").GetComponent<RectTransform>().lossyScale.z);
            inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(12,-12);
            inst.transform.Find("Text").GetComponent<Text>().text = bids[str].ToString();
        UpdateTexts();
        }
    }
    public void ResetBids(){
        balans += bids.Sum(x=>x.Value);
        bank -= bids.Sum(x=>x.Value);
        bids.Clear();
        GameObject [] chipToDestroy = GameObject.FindGameObjectsWithTag("Chip");
        for(int i=0; i<chipToDestroy.Length;i++)
            Destroy(chipToDestroy[i]);
        UpdateTexts();
    }
    public void EndBids(){
        if(bids.Count!=0){
            GetWinInf();
            CheckPlayerBids();
            bids.Clear();
            UpdateTexts();
            RouletteAnim();
            GameObject [] chipToDestroy = GameObject.FindGameObjectsWithTag("Chip");
            for(int i=0; i<chipToDestroy.Length;i++)
                Destroy(chipToDestroy[i]);
        }
    }
    void FixedUpdate() {
        if(grRoulNow<grRoul){
            GameObject.Find("kazColesico").transform.Rotate(Vector3.forward * 1f * rotationSpeed * Time.deltaTime);
            grRoulNow+=rotationSpeed * Time.deltaTime;
        }
        if(grBallNow>grBall){
            GameObject.Find("kazBall").transform.Rotate(Vector3.forward * -1f * rotationSpeed * Time.deltaTime);
            grBallNow-=rotationSpeed * Time.deltaTime;
        }
        else{
            LT += Time.deltaTime;
            if(LT>=3)
                decor.SetActive(false);
        }
    }
    void RouletteAnim(){
        LT=0;
        decor.SetActive(true);
        GameObject.Find("kazColesico").transform.rotation = Quaternion.identity;
        GameObject.Find("kazBall").transform.rotation = Quaternion.identity;
        grRoulNow=0;
        grBallNow=0;
        grRoul=winCellRatio[winCell]*360/37/2+2*360;
        grBall=-(winCellRatio[winCell]*360/37/2+3*360);
    }
    void UpdateTexts(){
        bankText.text = "Баланс:\n"+balans.ToString();
        balansText.text = "Банк:\n"+bank.ToString();
    }
}
