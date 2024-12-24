using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    int maxHp = 3; //salute massima
    
    public int currentHp = 3; //salute corrente

    public UnityAction onTakeDamage; //crea un evento


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

    //funzione per subire danno
    public void TakeDamage()
    {
        if (isDeath()) return; //se è morto, esce
       
        currentHp--; //diminuisce 
        if(currentHp <= 0 )
        {
            currentHp = 0;
        }
        if(onTakeDamage != null)
        {
            //richiama l'evento
            onTakeDamage.Invoke();
        }
    }

    public void AddNewLife()
    {
        maxHp++;
        currentHp++;
    }
}
