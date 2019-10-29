import csv
import time
import random
import json

TopRBList = []
TopWRList = []

QBList = []
RBList = []
WRList = []
TEList = []
FLEXList = []

RB_combination = []
WR_combination = []
position_combination = []

used_QB = []
used_WR = []
used_RB = []
used_TE = []
used_FLEX =[]

already_in = 0
class Player(object):
    def __init__(self, name, position, team, price, projected, ppp, id):
        self.name = name
        self.position = position
        self.team = team
        self.price = float(price)
        self.projected = float(projected)
        self.ppp = ppp
        self.id = id

class RB_Starters(object):
    def __init__(self, RB1, RB2):
        self.RB1 = RB1
        self.RB2 = RB2
        self.projected = float(RB1.projected) + float(RB2.projected)
        self.price = float(RB1.price) + float(RB2.price)

class WR_Starters(object):
    def __init__(self, WR1, WR2, WR3):
        self.WR1 = WR1
        self.WR2 = WR2
        self.WR3 = WR3
        self.projected = float(WR1.projected) + float(WR2.projected) + float(WR3.projected)
        self.price = float(WR1.price) + float(WR2.price) + float(WR3.price)
        
class Position_Players(object):
    def __init__(self, RBs, WRs, TE, FLEX):
        self.WRs = WRs
        self.RBs = RBs
        self.TE = TE
        self.FLEX = FLEX
        self.projected = float(WRs.projected) + float(RBs.projected) + float(TE.projected) + float(FLEX.projected)
        self.price = float(WRs.price) + float(RBs.price) + float(TE.price) + float(FLEX.price)
        
class Lineup(object):
    def __init__(self, QB, RB, WR, TE, FLEX):#, DEF):
        self.QB = QB
        self.RB = RB
        self.WR = WR
        self.TE = TE
        self.FLEX = FLEX
        #self.DEF = DEF
        self.total_points = float(QB.projected) + RB.projected + WR.projected + float(TE.projected) + float(FLEX.projected)# + float(DEF.projected)
        self.total_cost = float(QB.price) + RB.price + WR.price + float(FLEX.price) + float(TE.price)# + float(DEF.price)


def clear_lists():
    TopRBList.clear()
    TopWRList.clear()
    QBList.clear()
    TEList.clear()
    FLEXList.clear()
    WR_combination.clear()
    RB_combination.clear()
    used_QB.clear()
    used_RB.clear()
    used_TE.clear()
    used_WR.clear()
    used_FLEX.clear()


def read_projections_csv(file_path):
    with open(file_path) as csvfile:
        readCSV = csv.reader(csvfile, delimiter=',')
        for row in readCSV:
            if row[0] == 'Id':
                continue
            player = Player(row[1], row[3], row[2], row[7], row[6], row[8], row[0])
            if player.position == 'qb':
                QBList.append(player)
            elif player.position == 'rb':
                RBList.append(player)
                TopRBList.append(player)
            elif player.position == 'wr':
                WRList.append(player)
                TopWRList.append(player)
            elif player.position == 'te':
                TEList.append(player)
            if player.position == 'rb' or player.position == 'wr' or player.position == 'te':
                FLEXList.append(player)

def build_combinations():
    used_RB_combo = []
    for r in range(len(TopRBList)):
        for t in range(len(RBList)):
            if(RBList[t].name in used_RB_combo):
                continue
            if(TopRBList[r].name == RBList[t].name):
                continue
            RB_combination.append(RB_Starters(TopRBList[r], RBList[t]))
        used_RB_combo.append(TopRBList[r].name)

    for w in range(len(TopWRList) - 1):
        for x in range(w + 1, len(TopWRList)):
            for y in range(len(WRList)):
                if(WRList[y].name == TopWRList[x].name or WRList[y].name == TopWRList[w].name):
                    continue
                WR_combination.append(WR_Starters(TopWRList[w], TopWRList[x], WRList[y]))

def get_flex(rb, wr, te):
    while True:
        names = [rb.RB1.name, rb.RB2.name, wr.WR1.name, wr.WR2.name, wr.WR3.name, te.name]
        flex = FLEXList[random.randint(0, len(FLEXList) -1)]
        if flex.name not in names:
            return flex


def make_lineup(max_price):
    while True:
        qb = QBList[random.randint(0, len(QBList) -1)]
        rb = RB_combination[random.randint(0, len(RB_combination) -1)]
        wr = WR_combination[random.randint(0, len(WR_combination) -1)]
        te = TEList[random.randint(0, len(TEList) - 1)]
        flex = get_flex(rb, wr, te) 
        #d = DList[random.randint(0, len(DList) - 1)]
        lu = Lineup(qb, rb, wr, te, flex)#, d)
        if lu.total_cost <= max_price:
            return lu

def display_top_lineups(lineups, lineup_string):
    print('Top Lineups: ')
    for i in range(len(lineups) -1, -1, -1):
        print('------' + str(i + 1) + '-------')
        print(f'QB:   {lineups[i].QB.projected:.5}\t {lineups[i].QB.name:<20}\t {lineups[i].QB.team}\t {lineups[i].QB.price}\t {lineups[i].QB.ppp:.6}\t')
        print(f'RB1:  {lineups[i].RB.RB1.projected:.5}\t {lineups[i].RB.RB1.name:<20}\t {lineups[i].RB.RB1.team}\t {lineups[i].RB.RB1.price}\t {lineups[i].RB.RB1.ppp:.6}\t')
        print(f'RB2:  {lineups[i].RB.RB2.projected:.5}\t {lineups[i].RB.RB2.name:<20}\t {lineups[i].RB.RB2.team}\t {lineups[i].RB.RB2.price}\t {lineups[i].RB.RB2.ppp:.6}\t')
        print(f'WR1:  {lineups[i].WR.WR1.projected:.5}\t {lineups[i].WR.WR1.name:<20}\t {lineups[i].WR.WR1.team}\t {lineups[i].WR.WR1.price}\t {lineups[i].WR.WR1.ppp:.6}\t')
        print(f'WR2:  {lineups[i].WR.WR2.projected:.5}\t {lineups[i].WR.WR2.name:<20}\t {lineups[i].WR.WR2.team}\t {lineups[i].WR.WR2.price}\t {lineups[i].WR.WR2.ppp:.6}\t')
        print(f'WR3:  {lineups[i].WR.WR3.projected:.5}\t {lineups[i].WR.WR3.name:<20}\t {lineups[i].WR.WR3.team}\t {lineups[i].WR.WR3.price}\t {lineups[i].WR.WR3.ppp:.6}\t ')
        print(f'TE:   {lineups[i].TE.projected:.5}\t {lineups[i].TE.name:<20}\t {lineups[i].TE.team}\t {lineups[i].TE.price}\t {lineups[i].TE.ppp:.6}\t ')
        print(f'FLEX: {lineups[i].FLEX.projected:.5}\t {lineups[i].FLEX.name:<20}\t {lineups[i].FLEX.team}\t {lineups[i].FLEX.price}\t {lineups[i].FLEX.ppp:.6}\t')
        print(f'\nDefense Budget: \t {60000.00 - lineups[i].total_cost}')
        #print(f'D: {lineups[i].DEF.name} {lineups[i].DEF.team} {lineups[i].DEF.price} {lineups[i].DEF.projected}')
        print(f'Total Cost: \t\t{lineups[i].total_cost}')
        print(f'Projected Points: \t{lineups[i].total_points:.6}')
        print('\n\n')
    print(lineup_string)

def write_lineups(lineups, text_file_to_write, csv_file_to_write, lineup_string):
    with open(text_file_to_write, 'w') as writer:
        writer.write(lineup_string)
        writer.write('Top Lineups: ')
        writer.write('\n')
        for i in range(len(lineups)):
            writer.write('------' + str(i + 1) + '-------')
            writer.write('\n')
            writer.write(f'QB:   {lineups[i].QB.projected:.5}\t {lineups[i].QB.name:<20}\t {lineups[i].QB.team}\t {lineups[i].QB.price}\t {lineups[i].QB.ppp:.6}\n')
            writer.write(f'RB1:  {lineups[i].RB.RB1.projected:.5}\t {lineups[i].RB.RB1.name:<20}\t {lineups[i].RB.RB1.team}\t {lineups[i].RB.RB1.price}\t {lineups[i].RB.RB1.ppp:.6}\n')
            writer.write(f'RB2:  {lineups[i].RB.RB2.projected:.5}\t {lineups[i].RB.RB2.name:<20}\t {lineups[i].RB.RB2.team}\t {lineups[i].RB.RB2.price}\t {lineups[i].RB.RB2.ppp:.6}\n')
            writer.write(f'WR1:  {lineups[i].WR.WR1.projected:.5}\t {lineups[i].WR.WR1.name:<20}\t {lineups[i].WR.WR1.team}\t {lineups[i].WR.WR1.price}\t {lineups[i].WR.WR1.ppp:.6}\n')
            writer.write(f'WR2:  {lineups[i].WR.WR2.projected:.5}\t {lineups[i].WR.WR2.name:<20}\t {lineups[i].WR.WR2.team}\t {lineups[i].WR.WR2.price}\t {lineups[i].WR.WR2.ppp:.6}\n')
            writer.write(f'WR3:  {lineups[i].WR.WR3.projected:.5}\t {lineups[i].WR.WR3.name:<20}\t {lineups[i].WR.WR3.team}\t {lineups[i].WR.WR3.price}\t {lineups[i].WR.WR3.ppp:.6}\n')
            writer.write(f'TE:   {lineups[i].TE.projected:.5}\t {lineups[i].TE.name:<20}\t {lineups[i].TE.team}\t {lineups[i].TE.price}\t {lineups[i].TE.ppp:.6}\n')
            writer.write(f'FLEX: {lineups[i].FLEX.projected:.5}\t {lineups[i].FLEX.name:<20}\t {lineups[i].FLEX.team}\t {lineups[i].FLEX.price}\t {lineups[i].FLEX.ppp:.6}\t {lineups[i].FLEX.position}\n')
            writer.write(f'\nDefense Budget: {60000.00 - lineups[i].total_cost}\n')
            writer.write(f'Total Cost: {lineups[i].total_cost}\n')
            writer.write(f'Projected Points: {lineups[i].total_points}\n\n\n')
    with open(csv_file_to_write, 'w', newline='') as csvfile:
        fieldnames = ['QB','RB1','RB2','WR1', 'WR2', 'WR3', 'TE', 'FLEX', 'DEF', 'DEF Budget', 'Points', 'Total Cost']
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
        writer.writeheader()
        for lineup in lineups:
            writer.writerow({'QB' : f'{lineup.QB.id}:{lineup.QB.name}','RB1' : f'{lineup.RB.RB1.id}:{lineup.RB.RB1.name}',
                                'RB2': f'{lineup.RB.RB2.id}:{lineup.RB.RB2.name}', 'WR1' : f'{lineup.WR.WR1.id}:{lineup.WR.WR1.name}',
                                'WR2' : f'{lineup.WR.WR2.id}:{lineup.WR.WR2.name}', 'WR3' : f'{lineup.WR.WR3.id}:{lineup.WR.WR3.name}', 
                                'TE' : f'{lineup.TE.id}:{lineup.TE.name}', 'FLEX' : f'{lineup.FLEX.id}:{lineup.FLEX.name}', 'DEF':'',
                                'DEF Budget' : str(60000 - lineup.total_cost), 'Points' : lineup.total_points, 'Total Cost' : lineup.total_cost})

def write_lineups_json(lineups, text_file_to_write):
    with open(text_file_to_write, 'w') as outfile:
        json.dump(lineups, outfile)

def sort_lineups(lineups, keep):
    rtn_lineups = []
    used_lineups = []
    global already_in
    lineups = sorted(lineups,key=lambda x: x.total_points, reverse=True)
    for lineup in lineups:
        lineup_string = ''.join(sorted(lineup.QB.name + lineup.RB.RB1.name + lineup.RB.RB2.name + lineup.WR.WR1.name + lineup.WR.WR2.name + lineup.WR.WR3.name + lineup.TE.name + lineup.FLEX.name + lineup.QB.id + lineup.RB.RB1.id + lineup.RB.RB2.id + lineup.WR.WR1.id + lineup.WR.WR2.id + lineup.WR.WR3.id + lineup.TE.id + lineup.FLEX.id))
        if lineup_string not in used_lineups:
            rtn_lineups.append(lineup)
            used_lineups.append(lineup_string)
        else:
            print(f'Already in')
            already_in = already_in + 1
        if len(rtn_lineups) >= keep:
            break

    return rtn_lineups

def run_generator(config):
    loop_num = 0
    display_num = 0
    
    read_projections_csv(config['ReadFile'])
    top_lineups = []
    total_loops = 0
    initial = True
    while True:
        print('optimizing lineups...')
        if not initial:
            optimize_top_lineups(top_lineups)
        initial = False
        writes_til_read_new = config['WTNR']
        build_combinations()
        print(f'QBs: {len(QBList)}\nRBs: {len(TopRBList)}\nWRs: {len(TopWRList)}\nRB Combos: {len(RB_combination)}\nWR Combos: {len(WR_combination)}\nTE: {len(TEList)}\nFLEX: {len(FLEXList)}')
        while writes_til_read_new != 0:
            lineup = make_lineup(config['MaxPrice'])
            top_lineups.append(lineup)
            loop_num = loop_num + 1
            if loop_num == 1000000:
                print(f'sorting {len(top_lineups)}')
                top_lineups = sort_lineups(top_lineups, config['Lineups'])
                print(f'done {len(top_lineups)}')
                total_loops = total_loops + 1
                loop_num = 0
                display_num = display_num + 1
                display_totals(total_loops, display_num, writes_til_read_new)
                if(display_num == 10):
                    player_list = build_lineup_player_string(top_lineups)
                    print(player_list)
                    display_top_lineups(top_lineups, player_list)
                    print(f'QBs: {len(QBList)}\nRBs: {len(TopRBList)}\nWRs: {len(TopWRList)}\nRB Combos: {len(RB_combination)}\nWR Combos: {len(WR_combination)}\nTE: {len(TEList)}\nFLEX: {len(FLEXList)}')
                    print(f'{already_in} duplicated lineups')
                    display_num = 0
                    writes_til_read_new = writes_til_read_new - 1
                    print('writing lineups')
                    write_lineups(top_lineups, config['LineupOutput'], config['CSVOutput'], player_list)
                    print(f'----------WTNR {writes_til_read_new}------------')
                    # print('writing json')
                    # write_lineups_json(top_lineups, 'top_lineups_week_03_json.txt')
                    print('done')
                    time.sleep(2)


def display_totals(loops, display, wtrn):
    print(f'\nLineups Generated: {loops} Million')
    print(f'Display Loop: {display}')
    print(f'Writes til new read: {wtrn}\n\n')

def optimize_top_lineups(top_lineups):
    print(f'before QBs: {len(QBList)}\nRBs: {len(TopRBList)}\nWRs: {len(TopWRList)}\nRB Combos: {len(RB_combination)}\nWR Combos: {len(WR_combination)}\nTE: {len(TEList)}\nFLEX: {len(FLEXList)}')
    clear_lists()
    for lineup in top_lineups:
        if lineup.QB.name not in used_QB:
            QBList.append(lineup.QB)
            used_QB.append(lineup.QB.name)
        if lineup.RB.RB1.name not in used_RB:
            TopRBList.append(lineup.RB.RB1)
            used_RB.append(lineup.RB.RB1.name)
        if lineup.RB.RB2.name not in used_RB:
            TopRBList.append(lineup.RB.RB2)
            used_RB.append(lineup.RB.RB2.name)
        if lineup.WR.WR1.name not in used_WR:
            TopWRList.append(lineup.WR.WR1)
            used_WR.append(lineup.WR.WR1.name)
        if lineup.WR.WR2.name not in used_WR:
            TopWRList.append(lineup.WR.WR2)
            used_WR.append(lineup.WR.WR2.name)
        if lineup.WR.WR3.name not in used_WR:
            TopWRList.append(lineup.WR.WR3)
            used_WR.append(lineup.WR.WR3.name)
        if lineup.TE.name not in used_TE:
            TEList.append(lineup.TE)
            used_TE.append(lineup.TE.name)
        add_flex(lineup)
    for flex in used_FLEX:
        print(flex)
    print(f'after QBs: {len(QBList)}\nRBs: {len(TopRBList)}\nWRs: {len(TopWRList)}\nRB Combos: {len(RB_combination)}\nWR Combos: {len(WR_combination)}\nTE: {len(TEList)}\nFLEX: {len(FLEXList)}')


def add_flex(lineup):
    if lineup.RB.RB1.name not in used_FLEX:
        FLEXList.append(lineup.RB.RB1)
        used_FLEX.append(lineup.RB.RB1.name)
    if lineup.RB.RB2.name not in used_FLEX:
        FLEXList.append(lineup.RB.RB2)
        used_FLEX.append(lineup.RB.RB2.name)
    if lineup.WR.WR1.name not in used_FLEX:
        FLEXList.append(lineup.WR.WR1)
        used_FLEX.append(lineup.WR.WR1.name)
    if lineup.WR.WR2.name not in used_FLEX:
        FLEXList.append(lineup.WR.WR2)
        used_FLEX.append(lineup.WR.WR2.name)
    if lineup.WR.WR3.name not in used_FLEX:
        FLEXList.append(lineup.WR.WR3)
        used_FLEX.append(lineup.WR.WR3.name)
    if lineup.TE.name not in used_FLEX:
        FLEXList.append(lineup.TE)
        used_FLEX.append(lineup.TE.name)

def build_lineup_player_string(lineups):
    qb_used = []
    te_used = []
    wr_used = []
    rb_used = []
    wr_string = ''
    qb_string = ''
    te_string = ''
    rb_string = ''
    for lineup in lineups:
        if lineup.QB.name not in qb_used:
            qb_string = qb_string  + lineup.QB.name + ', '
            qb_used.append(lineup.QB.name)
        if lineup.RB.RB1.name not in rb_used:
            rb_string = rb_string + lineup.RB.RB1.name + ', '
            rb_used.append(lineup.RB.RB1.name)
        if lineup.RB.RB2.name not in rb_used:
            rb_string = rb_string + lineup.RB.RB2.name + ', '
            rb_used.append(lineup.RB.RB2.name)
        if lineup.WR.WR1.name not in wr_used:
            wr_string = wr_string + lineup.WR.WR1.name + ', ' 
            wr_used.append(lineup.WR.WR1.name)
        if lineup.WR.WR2.name not in wr_used:
            wr_string = wr_string + lineup.WR.WR2.name + ', '
            wr_used.append(lineup.WR.WR2.name)
        if lineup.WR.WR3.name not in wr_used:
            wr_string = wr_string + lineup.WR.WR3.name + ', '
            wr_used.append(lineup.WR.WR3.name)
        if lineup.TE.name not in te_used:
            te_string = te_string  + lineup.TE.name + ', '
            te_used.append(lineup.TE.name)
        if lineup.FLEX.position in 'TE':
            if lineup.FLEX.name not in te_used:
                te_string = te_string  + lineup.FLEX.name + ', '
                te_used.append(lineup.FLEX.name)
        if lineup.FLEX.position in 'RB':
            if lineup.FLEX.name not in rb_used:
                rb_string = rb_string + lineup.FLEX.name + ','
                rb_used.append(lineup.FLEX.name)
        if lineup.FLEX.position in 'WR':        
            if lineup.WR.WR3.name not in wr_used:
                wr_string = wr_string + lineup.FLEX.name + ','
                wr_used.append(lineup.FLEX.name)
    qb_string = qb_string[0:len(qb_string) - 2]
    wr_string = wr_string[0:len(wr_string) - 2]
    rb_string = rb_string[0:len(rb_string) - 2]
    te_string = te_string[0:len(te_string) - 2]
    rtn_string = f'QB: {qb_string}\nRB: {rb_string}\nWR: {wr_string}\nTE: {te_string}\n'
    return rtn_string
    



        

def read_config(path):
    config = {}
    max_price = 60000
    d_budget = 3700
    with open(path) as txtfile:
        reader = txtfile.readlines()
        for read in reader:
            line = read.rstrip().split('::')
            if line[0] == 'Lineups':
                config['Lineups'] = float(line[1])
            elif 'Writes til' in line[0]:
                config['WTNR'] = float(line[1])
            elif 'File to read' in line[0]:
                config['ReadFile'] = line[1]
            elif 'Top Lineup output path' in line[0]:
                config['LineupOutput'] = line[1]
            elif 'Top Lineup CSV output path' in line[0]:
                config['CSVOutput'] = line[1]
            elif 'Max Price' in line[0]:
                max_price = float(line[1])
            elif 'Defense Budget' in line[0]:
                d_budget = float(line[1])
    
    config["MaxPrice"] = max_price - d_budget

    print(config)
    return config
 
def main():
    config = read_config('C:\\Users\\15133\\Documents\\dfs\\Football\\dfs-config.txt')
    run_generator(config)

if __name__ == '__main__':
    main()
