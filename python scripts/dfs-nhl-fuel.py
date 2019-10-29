import csv
import time
import random
import json

TopCList = []
TopWList = []
TopDList = []
TopGList = []

CList = []
WList = []
DList = []
GList = []

C_combination = []
W_combination = []
D_combination = []


used_C = []
used_W = []
used_D = []
used_G = []
used_ =[]

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

class C_Starters(object):
    def __init__(self, C1, C2):
        self.C1 = C1
        self.C2 = C2
        self.projected = float(C1.projected) + float(C2.projected)
        self.price = float(C1.price) + float(C2.price)

class D_Starters(object):
    def __init__(self, D1, D2):
        self.D1 = D1
        self.D2 = D2
        self.projected = float(D1.projected) + float(D2.projected)
        self.price = float(D1.price) + float(D2.price)

class W_Starters(object):
    def __init__(self, W1, W2, W3, W4):
        self.W1 = W1
        self.W2 = W2
        self.W3 = W3
        self.W4 = W4
        self.projected = float(W1.projected) + float(W2.projected) + float(W3.projected) + float(W4.projected)
        self.price = float(W1.price) + float(W2.price) + float(W3.price) + float(W4.price)
        
class Lineup(object):
    def __init__(self, C, D, W, G):#, DEF):
        self.C = C
        self.D = D
        self.W = W
        self.G = G
        self.total_points = float(C.projected) + float(D.projected) + float(W.projected) + float(G.projected)
        self.total_cost = float(C.price) + float(D.price) + float(W.price) + float(G.price)


def clear_lists():
    TopCList.clear()
    TopWList.clear()
    TopDList.clear()
    C_combination.clear()
    D_combination.clear()
    W_combination.clear()
    used_C.clear()
    used_D.clear()
    used_G.clear()
    used_W.clear()


def read_projections_csv(file_path):
    with open(file_path) as csvfile:
        readCSV = csv.reader(csvfile, delimiter=',')
        for row in readCSV:
            if row[0] == 'first_name':
                continue
            player = Player(row[0] + ' ' + row[1], row[2], row[7], row[12], row[20], row[21], row[1])
            if row[2] == 'G':
                TopGList.append(player)
            elif row[2] == 'D':
                TopDList.append(player)
            elif row[2] == 'W':
                TopWList.append(player)
            elif row[2] == 'C':
                TopCList.append(player)

def build_combinations():
#    used_C_combo = []
    for r in range(len(TopCList) - 1):
        for t in range(r + 1, len(TopCList)):
 #           if(TopCList[t].name in used_C_combo):
  #              continue
   #         if(TopCList[r].name == TopCList[t].name):
    #            continue
            C_combination.append(C_Starters(TopCList[r], TopCList[t]))
   #     used_C_combo.append(TopRBList[r].name)

   
    for r in range(len(TopDList) - 1):
        for t in range(r + 1, len(TopDList)):
            D_combination.append(D_Starters(TopDList[r], TopDList[t]))

    for w in range(len(TopWList) - 3):
        for x in range(w + 1, len(TopWList) - 2):
            for y in range(x + 1, len(TopWList) - 1):
                for z in range(y + 1, len(TopWList)):
                    if(TopWList[y].name == TopWList[x].name or TopWList[y].name == TopWList[w].name):
                        continue
                    W_combination.append(W_Starters(TopWList[w], TopWList[x], TopWList[y], TopWList[z]))

def make_lineup(max_price):
    while True:
        c = C_combination[random.randint(0, len(C_combination) -1)]
        d = D_combination[random.randint(0, len(D_combination) -1)]
        w = W_combination[random.randint(0, len(W_combination) -1)]
        g = TopGList[random.randint(0, len(TopGList) - 1)]
        lu = Lineup(c, d, w, g)
        if lu.total_cost <= max_price:
            return lu

def display_top_lineups(lineups, lineup_string):
    print('Top Lineups: ')
    for i in range(len(lineups) -1, -1, -1):
        print('------' + str(i + 1) + '-------')
        print(f'D1:  {lineups[i].D.D1.projected:.5}\t {lineups[i].D.D1.name:<20}\t {lineups[i].D.D1.team}\t {lineups[i].D.D1.price}\t {lineups[i].D.D1.ppp:.6}\t')
        print(f'D2:  {lineups[i].D.D2.projected:.5}\t {lineups[i].D.D2.name:<20}\t {lineups[i].D.D2.team}\t {lineups[i].D.D2.price}\t {lineups[i].D.D2.ppp:.6}\t')
        print(f'W1:  {lineups[i].W.W1.projected:.5}\t {lineups[i].W.W1.name:<20}\t {lineups[i].W.W1.team}\t {lineups[i].W.W1.price}\t {lineups[i].W.W1.ppp:.6}\t')
        print(f'W2:  {lineups[i].W.W2.projected:.5}\t {lineups[i].W.W2.name:<20}\t {lineups[i].W.W2.team}\t {lineups[i].W.W2.price}\t {lineups[i].W.W2.ppp:.6}\t')
        print(f'W3:  {lineups[i].W.W3.projected:.5}\t {lineups[i].W.W3.name:<20}\t {lineups[i].W.W3.team}\t {lineups[i].W.W3.price}\t {lineups[i].W.W3.ppp:.6}\t ')
        print(f'W4:  {lineups[i].W.W4.projected:.5}\t {lineups[i].W.W4.name:<20}\t {lineups[i].W.W4.team}\t {lineups[i].W.W4.price}\t {lineups[i].W.W4.ppp:.6}\t ')
        print(f'C1:  {lineups[i].C.C1.projected:.5}\t {lineups[i].C.C1.name:<20}\t {lineups[i].C.C1.team}\t {lineups[i].C.C1.price}\t {lineups[i].C.C1.ppp:.6}\t')
        print(f'C2:  {lineups[i].C.C2.projected:.5}\t {lineups[i].C.C2.name:<20}\t {lineups[i].C.C2.team}\t {lineups[i].C.C2.price}\t {lineups[i].C.C2.ppp:.6}\t')
        print(f'G:   {lineups[i].G.projected:.5}\t {lineups[i].G.name:<20}\t {lineups[i].G.team}\t {lineups[i].G.price}\t {lineups[i].G.ppp:.6}\t')
        print(f'Total Cost: \t\t{lineups[i].total_cost}')
        print(f'Projected Points: \t{lineups[i].total_points:.6}')
        print('\n\n')
    #print(lineup_string)

def write_lineups(lineups, text_file_to_write, csv_file_to_write, lineup_string):
    with open(text_file_to_write, 'w') as writer:
        writer.write(lineup_string)
        writer.write('Top Lineups: ')
        writer.write('\n')
        for i in range(len(lineups)):
            writer.write('------' + str(i + 1) + '-------')
            writer.write(f'\nD1:  {lineups[i].D.D1.projected:.5}\t {lineups[i].D.D1.name:<20}\t {lineups[i].D.D1.team}\t {lineups[i].D.D1.price}\t {lineups[i].D.D1.ppp:.6}\t')
            writer.write(f'\nD2:  {lineups[i].D.D2.projected:.5}\t {lineups[i].D.D2.name:<20}\t {lineups[i].D.D2.team}\t {lineups[i].D.D2.price}\t {lineups[i].D.D2.ppp:.6}\t')
            writer.write(f'\nW1:  {lineups[i].W.W1.projected:.5}\t {lineups[i].W.W1.name:<20}\t {lineups[i].W.W1.team}\t {lineups[i].W.W1.price}\t {lineups[i].W.W1.ppp:.6}\t')
            writer.write(f'\nW2:  {lineups[i].W.W2.projected:.5}\t {lineups[i].W.W2.name:<20}\t {lineups[i].W.W2.team}\t {lineups[i].W.W2.price}\t {lineups[i].W.W2.ppp:.6}\t')
            writer.write(f'\nW3:  {lineups[i].W.W3.projected:.5}\t {lineups[i].W.W3.name:<20}\t {lineups[i].W.W3.team}\t {lineups[i].W.W3.price}\t {lineups[i].W.W3.ppp:.6}\t ')
            writer.write(f'\nW4:  {lineups[i].W.W4.projected:.5}\t {lineups[i].W.W4.name:<20}\t {lineups[i].W.W4.team}\t {lineups[i].W.W4.price}\t {lineups[i].W.W4.ppp:.6}\t ')
            writer.write(f'\nC1:  {lineups[i].C.C1.projected:.5}\t {lineups[i].C.C1.name:<20}\t {lineups[i].C.C1.team}\t {lineups[i].C.C1.price}\t {lineups[i].C.C1.ppp:.6}\t')
            writer.write(f'\nC2:  {lineups[i].C.C2.projected:.5}\t {lineups[i].C.C2.name:<20}\t {lineups[i].C.C2.team}\t {lineups[i].C.C2.price}\t {lineups[i].C.C2.ppp:.6}\t')
            writer.write(f'\nG:   {lineups[i].G.projected:.5}\t {lineups[i].G.name:<20}\t {lineups[i].G.team}\t {lineups[i].G.price}\t {lineups[i].G.ppp:.6}\t')
            writer.write(f'\nTotal Cost: \t\t{lineups[i].total_cost}')
            writer.write(f'\nProjected Points: \t{lineups[i].total_points:.6}')
            writer.write('\n\n')

 #   with open(csv_file_to_write, 'w', newline='') as csvfile:
  #      fieldnames = ['QB','RB1','RB2','WR1', 'WR2', 'WR3', 'TE', 'FLEX', 'DEF', 'DEF Budget', 'Points', 'Total Cost']
   #     writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
    #    writer.writeheader()
     #   for lineup in lineups:
      #      writer.writerow({'QB' : f'{lineup.QB.id}:{lineup.QB.name}','RB1' : f'{lineup.RB.RB1.id}:{lineup.RB.RB1.name}',
       #                         'RB2': f'{lineup.RB.RB2.id}:{lineup.RB.RB2.name}', 'WR1' : f'{lineup.WR.WR1.id}:{lineup.WR.WR1.name}',
        #                        'WR2' : f'{lineup.WR.WR2.id}:{lineup.WR.WR2.name}', 'WR3' : f'{lineup.WR.WR3.id}:{lineup.WR.WR3.name}', 
         #                       'TE' : f'{lineup.TE.id}:{lineup.TE.name}', 'FLEX' : f'{lineup.FLEX.id}:{lineup.FLEX.name}', 'DEF':'',
          #                      'DEF Budget' : str(60000 - lineup.total_cost), 'Points' : lineup.total_points, 'Total Cost' : lineup.total_cost})

def write_lineups_json(lineups, text_file_to_write):
    with open(text_file_to_write, 'w') as outfile:
        json.dump(lineups, outfile)

def sort_lineups(lineups, keep):
    rtn_lineups = []
    used_lineups = []
    global already_in
    lineups = sorted(lineups,key=lambda x: x.total_points, reverse=True)
    for lineup in lineups:
        lineup_string = ''.join(sorted(lineup.G.name + lineup.D.D1.name + lineup.D.D2.name + lineup.W.W1.name + lineup.W.W2.name + lineup.W.W3.name + lineup.W.W4.name + lineup.C.C1.name + lineup.C.C2.name + lineup.G.id + lineup.D.D1.id + lineup.D.D2.id + lineup.W.W1.id + lineup.W.W2.id + lineup.W.W3.id + lineup.W.W4.id + lineup.C.C1.id + lineup.C.C2.id ))
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
        print(f'Gs: {len(TopGList)}\nDs: {len(TopDList)}\nWs: {len(TopWList)}\nCs: {len(TopCList)}\nD Combos: {len(D_combination)}\nW Combos: {len(W_combination)}\nC Combos: {len(C_combination)}\n')
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
                    player_list = ''#build_lineup_player_string(top_lineups)
                    print(player_list)
                    display_top_lineups(top_lineups, player_list)
                    print(f'Gs: {len(TopGList)}\nDs: {len(TopDList)}\nWs: {len(TopWList)}\nCs: {len(TopCList)}\nD Combos: {len(D_combination)}\nW Combos: {len(W_combination)}\nC Combos: {len(C_combination)}\n')
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
    print(f'before Gs: {len(TopGList)}\nDs: {len(TopDList)}\nWs: {len(TopWList)}\nCs: {len(TopCList)}\nD Combos: {len(D_combination)}\nW Combos: {len(W_combination)}\nC Combos: {len(C_combination)}\n')
    clear_lists()
    for lineup in top_lineups:
        if lineup.C.C1.name not in used_C:
            TopCList.append(lineup.C.C1)
            used_C.append(lineup.C.C1.name)
        if lineup.C.C2.name not in used_C:
            TopCList.append(lineup.C.C2)
            used_C.append(lineup.C.C2.name)
        if lineup.D.D1.name not in used_D:
            TopDList.append(lineup.D.D1)
            used_D.append(lineup.D.D1.name)
        if lineup.D.D2.name not in used_D:
            TopDList.append(lineup.D.D2)
            used_D.append(lineup.D.D2.name)
        if lineup.W.W1.name not in used_W:
            TopWList.append(lineup.W.W1)
            used_W.append(lineup.W.W1.name)
        if lineup.W.W2.name not in used_W:
            TopWList.append(lineup.W.W2)
            used_W.append(lineup.W.W2.name)
        if lineup.W.W3.name not in used_W:
            TopWList.append(lineup.W.W3)
            used_W.append(lineup.W.W3.name)
        if lineup.W.W4.name not in used_W:
            TopWList.append(lineup.W.W4)
            used_W.append(lineup.W.W4.name)
    print(f'after Gs: {len(TopGList)}\nDs: {len(TopDList)}\nWs: {len(TopWList)}\nCs: {len(TopCList)}\nD Combos: {len(D_combination)}\nW Combos: {len(W_combination)}\nC Combos: {len(C_combination)}\n')


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
    config = read_config('C:\\Users\\15133\\Documents\\dfs\\Hockey\\dfs-nhl-config.txt')
    run_generator(config)

if __name__ == '__main__':
    main()
