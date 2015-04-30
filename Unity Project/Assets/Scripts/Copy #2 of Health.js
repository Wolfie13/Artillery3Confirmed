#pragma strict

var curHealth : int = 100;
var maxHealth : int = 100;
var healthtext : GUIText;
 
function Start () {
        healthRegen();
}
 
function Update () {
 
        healthtext.text = curHealth + " / " + maxHealth;
 
        if(curHealth <= 0 ) {
                curHealth = 0;       
        }
        if(curHealth == 0)
        {
        	Destroy(gameObject);
        }
        
        
 
        if(curHealth > 100) {
                curHealth = 100;
        }
 
        if(Input.GetKeyDown("e")) {
                curHealth -= 10;
        }
}
 
function healthRegen () {
 
 for(i=1;i>0;i++) {
                yield WaitForSeconds(20);
 
                if(curHealth < maxHealth) {
                        curHealth+10;
                }
        }
}