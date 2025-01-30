using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    
    public int maxHp = 3; //salute massima
    
    public int currentHp = 3; //salute corrente

    public UnityAction onTakeDamage; //crea un evento

    [SerializeField]
    float invincibleTime = 1;
    float counter = 0;
    bool invincible = false;

    public bool isDeath()
    {
        return currentHp <= 0;     
    }

    //funzione cura
    public void Heal()
    {
        currentHp++;
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
    }
    private void Update()
    {
        //invincibilità
        if (invincible) 
        { 
            counter-=Time.deltaTime;
            if(counter <= 0)
            {
                invincible = false;
            }
        }
    }

    //funzione per subire danno
    public void TakeDamage()
    {
        if (isDeath() || invincible) return; //se è morto, esce
       
        currentHp--; //diminuisce 
        invincible = true; //invincibilità
        counter = invincibleTime;
        if(currentHp <= 0 )
        {
            currentHp = 0;
        }
        if(onTakeDamage != null)
        {
            //richiama l'evento
            onTakeDamage.Invoke();
        }
        //attiviamo il suono
        if(SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound("hit");
        }
    }

    public void AddNewLife()
    {
        maxHp++;
        currentHp++;
    }

    public void RestoreLife()
    {
        currentHp = maxHp;
    }
}
