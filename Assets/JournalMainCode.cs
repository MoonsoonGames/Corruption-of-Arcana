using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda;

public class JournalMainCode : MonoBehaviour
{
    #region Game Objects 
        private int currentPage = 0;

        #region Sections
        [Header("Sections and SubTabs")]
        public GameObject[] JournalSections;
        public GameObject[] MainTabs;
        public GameObject[] Subtabs;
        #endregion

        #region Main Page 
        public GameObject MainJournalPage;
        #endregion

        #region Bestiary Pages 
        [Header("Bestiary Pages")]
        public GameObject BestiarySection;
        public GameObject MainBestiaryPage;
        public GameObject[] speciesSection;

            #region Constructs
            [Header("Constructs Pages")]
            public GameObject[] ConstructPages;
            #endregion

            #region Undead
            [Header("Undead Pages")]
            public GameObject[] UndeadPages;
            #endregion

            #region Beasts
            [Header("Beasts Pages")]
            public GameObject[] BeastsPages;
            #endregion

            #region Thelmians
            [Header("Thelmians Pages")]
            public GameObject[] ThelmiansPages;
            #endregion

            #region Eldritch
            [Header("Eldritch Pages")]
            public GameObject[] EldritchPages;
            #endregion

        #endregion

        #region Curios Page 
        //public GameObject[] CurioTypes;
        #endregion 

    #endregion

    void TogglePages(GameObject[] pages, bool state)
    {
        foreach(GameObject obj in pages)
        {
            obj.SetActive(state);
        }
    }

    void Update()
    {
        //Home open
        if(JournalSections[0].activeSelf == false)
        {
            GameObject currentTab = MainTabs[0];
            currentTab.GetComponent<TabMovement>().enabled = true;

            currentTab.transform.localPosition = new Vector3(585, 310, 0);
        }
    }

    #region Error Avoidance 
    void ConstructErrorAvoidance()
    {
        if(currentPage >= ConstructPages.Length)
        {
            currentPage = 0;
        }
    }

    void UndeadErrorAvoidance()
    {
        if(currentPage >= UndeadPages.Length)
        {
            currentPage = 0;
        }
    }

    void BeastsErrorAvoidance()
    {
        if(currentPage >= BeastsPages.Length)
        {
            currentPage = 0;
        }
    }

    void ThelmiansErrorAvoidance()
    {
        if(currentPage >= ThelmiansPages.Length)
        {
            currentPage = 0;
        }
    }

    void EldritchErrorAvoidance()
    {
        if(currentPage >= EldritchPages.Length)
        {
            currentPage = 0;
        }
    }
    #endregion

    /* ---------- Divider ---------- */

    #region Home Tab 
    public void HomeBTN()
    {
        TogglePages(JournalSections, false); //turn off all sections
        GameObject Page = JournalSections[0];
        Page.SetActive(true);

        TogglePages(Subtabs, false);//off all subtabs 

        GameObject currentTab = MainTabs[0];
        currentTab.GetComponent<TabMovement>().enabled = false;
        currentTab.transform.localPosition = new Vector3(658.0594f, 310, 0);
    }
    #endregion

    /* ---------- Divider ---------- */

    #region Bestiary Tab 
    public void BestiaryBTN()
    {
        //turn off all journal sections and turn on the bestiary
        TogglePages(JournalSections, false);
        GameObject Page = JournalSections[1];
        Page.SetActive(true);

        //Turn on correct subtabs
        TogglePages(Subtabs, false);
        GameObject Tabs = Subtabs[0];
        Tabs.SetActive(true);

        //make sure the species pages are off
        TogglePages(ConstructPages, false);
        TogglePages(UndeadPages, false);
        TogglePages(BeastsPages, false);
        TogglePages(ThelmiansPages, false);
        TogglePages(EldritchPages, false);

        MainBestiaryPage.SetActive(true);
    }
    #endregion

    #region Bestiary Subtabs 
        #region Constructs 
        public void ConstructsBTN()
        {
            TogglePages(speciesSection, false);
            GameObject Section = speciesSection[0];
            Section.SetActive(true);

            MainBestiaryPage.SetActive(false);//turn off the main page
            //make sure the species pages are off
            TogglePages(ConstructPages, false);
            TogglePages(UndeadPages, false);
            TogglePages(BeastsPages, false);
            TogglePages(ThelmiansPages, false);
            TogglePages(EldritchPages, false);

            GameObject Page = ConstructPages[0];
            Page.SetActive(true);
            currentPage = 0;
        }
        #endregion

        #region Undead 
        public void UndeadBTN()
        {
            TogglePages(speciesSection, false);
            GameObject Section = speciesSection[1];
            Section.SetActive(true);

            MainBestiaryPage.SetActive(false);//turn off the main page
            //make sure the species pages are off
            TogglePages(ConstructPages, false);
            TogglePages(UndeadPages, false);
            TogglePages(BeastsPages, false);
            TogglePages(ThelmiansPages, false);
            TogglePages(EldritchPages, false);

            GameObject Page = UndeadPages[0];
            Page.SetActive(true);
            currentPage = 0;
        }
        #endregion

        #region Beasts 
        public void BeastsBTN()
        {
            TogglePages(speciesSection, false);
            GameObject Section = speciesSection[2];
            Section.SetActive(true);

            MainBestiaryPage.SetActive(false);//turn off the main page
            //make sure the species pages are off
            TogglePages(ConstructPages, false);
            TogglePages(UndeadPages, false);
            TogglePages(BeastsPages, false);
            TogglePages(ThelmiansPages, false);
            TogglePages(EldritchPages, false);

            GameObject Page = BeastsPages[0];
            Page.SetActive(true);
            currentPage = 0;
        }
        #endregion

        #region Thelmians 
        public void ThelmiansBTN()
        {
            TogglePages(speciesSection, false);
            GameObject Section = speciesSection[3];
            Section.SetActive(true);

            MainBestiaryPage.SetActive(false);//turn off the main page
            //make sure the species pages are off
            TogglePages(ConstructPages, false);
            TogglePages(UndeadPages, false);
            TogglePages(BeastsPages, false);
            TogglePages(ThelmiansPages, false);
            TogglePages(EldritchPages, false);

            GameObject Page = ThelmiansPages[0];
            Page.SetActive(true);
            currentPage = 0;
        }
        #endregion

        #region Eldritch 
        public void EldritchBTN()
        {
            TogglePages(speciesSection, false);
            GameObject Section = speciesSection[4];
            Section.SetActive(true);

            MainBestiaryPage.SetActive(false);//turn off the main page
            //make sure the species pages are off
            TogglePages(ConstructPages, false);
            TogglePages(UndeadPages, false);
            TogglePages(BeastsPages, false);
            TogglePages(ThelmiansPages, false);
            TogglePages(EldritchPages, false);

            GameObject Page = EldritchPages[0];
            Page.SetActive(true);
            currentPage = 0;
        }
        #endregion
    #endregion

    #region Page Buttons 
        #region Next Bestiary Page 
        public void NextPage()
        {
            if(speciesSection[0].activeSelf == true)
            {
                //close current, access next gameobject in array, turn on that page
                ConstructPages[currentPage].SetActive(false);
                currentPage++;
                ConstructErrorAvoidance();
                ConstructPages[currentPage].SetActive(true);
            }

            if(speciesSection[1].activeSelf == true)
            {
                UndeadPages[currentPage].SetActive(false);
                currentPage++;
                UndeadErrorAvoidance();
                UndeadPages[currentPage].SetActive(true);
            }

            if(speciesSection[2].activeSelf == true)
            {
                BeastsPages[currentPage].SetActive(false);
                currentPage++;
                BeastsErrorAvoidance();
                BeastsPages[currentPage].SetActive(true);
            }

            if(speciesSection[3].activeSelf == true)
            {
                ThelmiansPages[currentPage].SetActive(false);
                currentPage++;
                ThelmiansErrorAvoidance();
                ThelmiansPages[currentPage].SetActive(true);
            }

            if(speciesSection[4].activeSelf == true)
            {
                EldritchPages[currentPage].SetActive(false);
                currentPage++;
                EldritchErrorAvoidance();
                EldritchPages[currentPage].SetActive(true);
            }
        }
        #endregion

        #region Last Bestiary Page 
        public void LastPage()
        {
            if(speciesSection[0].activeSelf == true)
            {
                ConstructPages[currentPage].SetActive(false);
                currentPage--;
                ConstructErrorAvoidance();
                ConstructPages[currentPage].SetActive(true);
            }

            if(speciesSection[1].activeSelf == true)
            {
                UndeadPages[currentPage].SetActive(false);
                currentPage--;
                UndeadErrorAvoidance();
                UndeadPages[currentPage].SetActive(true);
            }

            if(speciesSection[2].activeSelf == true)
            {
                BeastsPages[currentPage].SetActive(false);
                currentPage--;
                BeastsErrorAvoidance();
                BeastsPages[currentPage].SetActive(true);
            }

            if(speciesSection[3].activeSelf == true)
            {
                ThelmiansPages[currentPage].SetActive(false);
                currentPage--;
                ThelmiansErrorAvoidance();
                ThelmiansPages[currentPage].SetActive(true);
            }

            if(speciesSection[4].activeSelf == true)
            {
                EldritchPages[currentPage].SetActive(false);
                currentPage--;
                EldritchErrorAvoidance();
                EldritchPages[currentPage].SetActive(true);
            }
        }
        #endregion
    #endregion
    /* ---------- Divider ---------- */

    #region Curios Tab 
    public void CuriosBTN()
    {
        //off all pages, on curios
        TogglePages(JournalSections, false);
        GameObject Page = JournalSections[2];
        Page.SetActive(true);

        //off all subtabs, on curios
        TogglePages(Subtabs, false);
        GameObject Tabs = Subtabs[1];
        Tabs.SetActive(true);
    }
    #endregion

    #region Curios Subtabs
        #region Trinkets 
        public void TrinketsBTN()
        {
            /*
            TogglePages(CurioTypes, false);
            GameObject Page = CurioTypes[0];
            Page.SetActive(true);
            */
        }
        #endregion

        #region Scrolls 
        public void ScrollsBTN()
        {
            /*
            TogglePages(CurioTypes, false);
            GameObject Page = CurioTypes[1];
            Page.SetActive(true);
            */
        }
        #endregion
    #endregion

    /* ---------- Divider ---------- */

    #region Quests Tab

    #endregion

    /* ---------- Divider ---------- */

    #region Feats Tab

    #endregion

    /* ---------- Divider ---------- */

    #region Index Tab

    #endregion

    /* ---------- Divider ---------- */
}