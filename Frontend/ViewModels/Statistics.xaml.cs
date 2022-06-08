using Buchungs_und_Planungssystem.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Buchungs_und_Planungssystem.Frontend.ViewModels
{
    /// <summary>
    /// Interaktionslogik für Statistics.xaml
    /// </summary>
    public partial class Statistics : UserControl
    {
        public Statistics()
        {
            InitializeComponent();
            HideGrids();
        }

        public bool MultipleDays { get; set; }
        int planned = 0;
        int booked = 0;
        int isAmount = 0;
        int seasonId = 2;
        DateTime today = DateTime.Today;
        double roomRevs = 0;
        double packRevs = 0;
        double total = 0;
        double StaffCosts = 0;
        double extraStaff = 0;
        double totalCosts = 0;
        double opsResult = 0;
        double costDriverCost = 0;
        double mult = 0;
        double roomCosts = 0;

        public void Evaluate(object sender, RoutedEventArgs e)
        {

            var startDate = StartDate.SelectedDate;
            var endDate = EndDate.SelectedDate;

            if (EndDate.IsEnabled == true && startDate != null && EndDate != null)
            {
                
                if(endDate > startDate)
                {
                    MultipleEvaluate(DateTime.Parse(startDate.ToString()), DateTime.Parse(EndDate.ToString()));
                }
                else
                {
                    MessageBox.Show("Das Enddatum darf nicht vor dem Anfangsdatum stattfinden");
                }
            }
            else if(EndDate.IsEnabled == false && startDate != null)
            {
                SingleEvaluate(DateTime.Parse(startDate.ToString()));
            }
            else
            {
                MessageBox.Show("Es muss ein gültiges Datum eingegeben werden");
            }
            
        }

       
        public void SingleEvaluate(DateTime date)
        {
            CostDriverList.Items.Clear();
            //Values
            var startDate = date;
            Booking booking = Booking.GetBookingByDate(startDate);  //Booking Object
            if(booking != null)
            {
                int planned = booking.Planned;
                int booked = booking.Booked;
                int isAmount = booking.IsCount;

                //Get Season
                SeasonTimespan season = SeasonTimespan.GetSeasonByDay(startDate);
                if(season != null)
                {
                    seasonId = season.SeasonId;
                }
                

                //Get booked rooms
                List<BookingRoom> bookedRooms = BookingRoom.GetBookingRoom(booking.Id);
                List<BookingExperiencePackage> bookedExpPackages = BookingExperiencePackage.GetBookingExpPack(booking.Id);

                if(bookedRooms != null)
                {
                    //Check if date is future or past
                    if (startDate <= today)
                    {
                        //past
                        mult = isAmount;
                            foreach (var rooms in bookedRooms)
                            {
                                RoomPrice roomPrice = RoomPrice.GetRoomPriceByIds(rooms.RoomId, seasonId, booking.LocationId);
                                if(roomPrice != null)
                                {
                                    double room = isAmount * roomPrice.Price;
                                    roomRevs += room;
                                }
                                
                            }
                            foreach (var packs in bookedExpPackages)
                            {
                                ExperiencePackage expPack = ExperiencePackage.GetPackById(packs.ExpPackageId);
                                if(expPack != null)
                                {
                                    double pack = isAmount * expPack.Price;
                                    packRevs += pack;
                                }
                                
                            }
                        //Get Extra Staff
                        int amount = isAmount + 10;
                        int isAmountFor = amount / 10;

                        for (int i = 0; i < isAmountFor; i++)
                        {
                            extraStaff = i;
                        }

                    }
                    else
                    {
                        //future
                        mult = planned;
                        //Room
                        foreach (var rooms in bookedRooms)
                            {
                                RoomPrice roomPrice = RoomPrice.GetRoomPriceByIds(rooms.RoomId, seasonId, booking.LocationId);
                                double room = planned * roomPrice.Price;
                                roomRevs += room;
                            }
                        

                            //Package
                            foreach (var packs in bookedExpPackages)
                            {
                                ExperiencePackage expPack = ExperiencePackage.GetPackById(packs.ExpPackageId);
                                if (expPack != null)
                                {
                                    double pack = planned * expPack.Price;
                                    packRevs += pack;
                                }

                            }
                        extraStaffText.Text = "empfohlene zusätzliche Mitarbeiter: ";
                        //Get Extra Staff
                        int amount = planned + 10;
                        int plannendAmountFor = amount / 10;

                        for (int i = 0; i < plannendAmountFor; i++)
                        {
                            extraStaff = i;
                        }
                    }
                    //Get Costs
                    
                    //Staff
                    List<BookingStaff> staffList = BookingStaff.GetBookingStaff(booking.Id);
                    foreach(var staff in staffList)
                    {
                        Staff staffObj = Staff.GetStaffById(staff.StaffId);
                        double hourWage = staffObj.Wage;
                        double staffWage = hourWage * 8;
                        StaffCosts += staffWage;
                    }

                    //Cost drivers
                    List<BookingCostDriver> costDrivers = BookingCostDriver.GetCostDrivers(booking.Id);
                    if (costDrivers != null)
                    {
                        foreach (var costDriver in costDrivers)
                        {
                            CostDriver cd = CostDriver.GetCostDriverById(costDriver.CostDriverId);
                            if(cd != null)
                            {
                                var cdName = cd.Designation;
                                var cdCost = cd.Costs;
                                double cdCosts = 0;
                                if (cd.Variable)
                                {
                                    cdCosts = cdCost * mult;
                                }
                                else
                                {
                                    cdCosts = cdCost;
                                }
                                CostDriverList.Items.Add(cdName + "         " + cdCosts + "€");

                                costDriverCost += cdCosts;
                            }
                            
                        }
                    }

                    //Get total revenues
                    total = packRevs + roomRevs;
                    //Display values
                    Date.Text = startDate.ToString();
                    till.Text = "";
                    DateEnd.Text = "";
                    Planned.Text = planned.ToString();
                    Booked.Text = booked.ToString();
                    IsAmount.Text = isAmount.ToString();
                    RoomRevs.Text = roomRevs.ToString() + "€";
                    ExpPackageRevs.Text = packRevs.ToString() + "€";
                    TotalRevs.Text = total.ToString() + "€";

                    //Costs
                    totalCosts = StaffCosts + costDriverCost;
                    opsResult = total - totalCosts;
                    CostDriverText.Text = costDriverCost.ToString();
                    FixStaffCosts.Text = StaffCosts.ToString() + "€";
                    extraStaffAmount.Text = extraStaff.ToString() + "€";
                    TotalCosts.Text = totalCosts.ToString() + "€";
                    
                    //Operating result
                    OpsResult.Text = opsResult.ToString() + "€";

                    //Show Grid
                    SingleDayGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Es wurden an diesem Tag keine Zimmer gebucht");
                }    
            }
            else
            {
                MessageBox.Show("Zu diesem Datum existiert noch kein Eintrag!");
            }
            
        }

        public void MultipleEvaluate(DateTime startDate, DateTime endDate)
        {
            CostDriverList.Items.Clear();
            
            IEnumerable<DateTime> daysBetween = GetDaysBetweenDateTimes(DateTime.Parse(startDate.ToString()), DateTime.Parse(endDate.ToString()));
            foreach (var day in daysBetween)
            {
                
                //Values
                Booking booking = Booking.GetBookingByDate(day);  //Booking Object
                if (booking != null)
                {
                    planned += booking.Planned;
                    booked += booking.Booked;
                    isAmount += booking.IsCount;

                    //Get Season
                    SeasonTimespan season = SeasonTimespan.GetSeasonByTimeSpan(startDate, endDate);
                    var seasonId = season.SeasonId;
                    //Get booked rooms
                    List<BookingRoom> bookedRooms = BookingRoom.GetBookingRoom(booking.Id);
                    List<BookingExperiencePackage> bookedExpPackages = BookingExperiencePackage.GetBookingExpPack(booking.Id);

                    if (bookedRooms != null)
                    {
                        //Check if date is future or past
                        if (day <= today)
                        {
                            //past
                            mult = isAmount;
                            foreach (var rooms in bookedRooms)
                            {
                                RoomPrice roomPrice = RoomPrice.GetRoomPriceByIds(rooms.RoomId, seasonId, booking.LocationId);
                                //if(roomPrice != null)
                                double room = isAmount * roomPrice.Price;
                                roomRevs += room;
                            }
                            foreach (var packs in bookedExpPackages)
                            {
                                ExperiencePackage expPack = ExperiencePackage.GetPackById(packs.ExpPackageId);
                                if (expPack != null)
                                {
                                    double pack = isAmount * expPack.Price;
                                    packRevs += pack;
                                }

                            }
                            //Get Extra Staff
                            int amount = isAmount + 10;
                            int isAmountFor = amount / 10;

                            for (int i = 0; i < isAmountFor; i++)
                            {
                                extraStaff = i;
                            }

                        }
                        else
                        {
                            //future
                            mult = planned;
                            //Room
                            foreach (var rooms in bookedRooms)
                            {
                                RoomPrice roomPrice = RoomPrice.GetRoomPriceByIds(rooms.RoomId, seasonId, booking.LocationId);
                                double room = planned * roomPrice.Price;
                                roomRevs += room;
                            }


                            //Package
                            foreach (var packs in bookedExpPackages)
                            {
                                ExperiencePackage expPack = ExperiencePackage.GetPackById(packs.ExpPackageId);
                                if (expPack != null)
                                {
                                    double pack = planned * expPack.Price;
                                    packRevs += pack;
                                }

                            }
                            extraStaffText.Text = "empfohlene zusätzliche Mitarbeiter: ";
                            //Get Extra Staff
                            int amount = planned + 10;
                            int plannendAmountFor = amount / 10;

                            for (int i = 0; i < plannendAmountFor; i++)
                            {
                                extraStaff = i;
                            }
                        }
                        //Get Costs

                        //Staff
                        List<BookingStaff> staffList = BookingStaff.GetBookingStaff(booking.Id);
                        foreach (var staff in staffList)
                        {
                            Staff staffObj = Staff.GetStaffById(staff.StaffId);
                            double hourWage = staffObj.Wage;
                            double staffWage = hourWage * 8;
                            StaffCosts += staffWage;
                        }

                        //Cost drivers
                        List<BookingCostDriver> costDrivers = BookingCostDriver.GetCostDrivers(booking.Id);
                        if (costDrivers != null)
                        {
                            CostDriverList.Items.Add(day);
                            foreach (var costDriver in costDrivers)
                            {
                                CostDriver cd = CostDriver.GetCostDriverById(costDriver.CostDriverId);
                                if (cd != null)
                                {

                                    var cdName = cd.Designation;
                                    var cdCost = cd.Costs;
                                    double cdCosts = 0;
                                    if (cd.Variable)
                                    {
                                        cdCosts = cdCost * mult;
                                    }
                                    else
                                    {
                                        cdCosts = cdCost;
                                    }
                                    CostDriverList.Items.Add(cdName + "         " + cdCosts + "€");

                                    costDriverCost += cdCosts;
                                }

                            }
                            
                        }
                        BookingCosts.SaveBookingCosts(new BookingCosts(booking.Id, costDriverCost, StaffCosts, total, false));
                    }
                    else
                    {
                        MessageBox.Show("Es wurden an diesem Tag keine Zimmer gebucht");
                    }
                }
                else
                {
                    MessageBox.Show("Zu diesem Datum existiert noch kein Eintrag!");
                }
            }//For each end

            
            //Get total revenues
            total = packRevs + roomRevs;
            //Display values
            Date.Text = startDate.ToShortDateString();
            DateEnd.Text = endDate.ToShortDateString();
            till.Text = " - "; 
            Planned.Text = planned.ToString();
            Booked.Text = booked.ToString();
            IsAmount.Text = isAmount.ToString();
            RoomRevs.Text = roomRevs.ToString() + "€";
            ExpPackageRevs.Text = packRevs.ToString() + "€";
            TotalRevs.Text = total.ToString() + "€";

            //Costs
            totalCosts = StaffCosts + costDriverCost;
            opsResult = total - totalCosts;
            CostDriverText.Text = costDriverCost.ToString() + "€";
            FixStaffCosts.Text = StaffCosts.ToString() + "€";
            extraStaffAmount.Text = extraStaff.ToString();
            TotalCosts.Text = totalCosts.ToString() + "€";

            //Operating result
            double result = Math.Round(opsResult, 2);
            OpsResult.Text = result.ToString() + "€";

            //Show Grid
            SingleDayGrid.Visibility = Visibility.Visible;
        }

        private void HideGrids()
        {
            SingleDayGrid.Visibility = Visibility.Hidden;
            MultipleDayGrid.Visibility = Visibility.Hidden;
        }

        public IEnumerable<DateTime> GetDaysBetweenDateTimes(DateTime start, DateTime end)
        {
            var newEnd = end.AddDays(+1);
            var days = newEnd - start;
            for (int i = 0; i < days.TotalDays; i++)
            {
                yield return start.AddDays(i);
            }
        }

        private void ToggleEndDate(object sender, RoutedEventArgs e)
        {
            if(EndDate.IsEnabled == false)
            {
                EndDate.IsEnabled = true;
                MultipleDays = true;
                SingleDayGrid.Visibility = Visibility.Hidden;
                MultipleDayGrid.Visibility = Visibility.Visible;
            }
            else
            {
                EndDate.IsEnabled = false;
                MultipleDays = false;
                MultipleDayGrid.Visibility = Visibility.Hidden;
                SingleDayGrid.Visibility = Visibility.Visible;
                
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
